using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.DTOs;
using ParkyAPI.Services.IRepositoryService;
using System.Collections.Generic;

namespace ParkyAPI.Controllers
{
    /// <summary>
    /// park controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class NationalParksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly INationalParkRepository _nationalParkRepository;


        public NationalParksController(INationalParkRepository nationalParkRepository,
            IMapper mapper)
        {
            _nationalParkRepository = nationalParkRepository;
            _mapper = mapper;
        }

        /// <summary>
        ///     Returns a list of all the national parks in the database
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<NationalPark>))]
        public IActionResult GetAllNationalParks()
        {
            var nationalParks = _nationalParkRepository.GetAllNationalParks();

            var nationalParksToReturn =
                _mapper.Map<IEnumerable<NationalPark>, IEnumerable<NationalParkDto>>(nationalParks);

            return Ok(nationalParksToReturn);
        }

        /// <summary>
        ///     Returns a national park with the specified id or null if none is found
        /// </summary>
        /// <param name="nationalParkId"></param>
        /// <returns></returns>
        [HttpGet("{nationalParkId:int}", Name = "GetNationalParkById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NationalParkDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetNationalParkById(int nationalParkId)
        {
            var nationalPark = _nationalParkRepository.GetNationalParkById(nationalParkId);

            if (nationalPark == null)
            {
                return NotFound();
            }

            var parkDto = _mapper.Map<NationalParkDto>(nationalPark);

            return Ok(parkDto);
        }

        /// <summary>
        ///     adds a national park to the existing record of national parks
        /// </summary>
        /// <param name="nationalParkDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(NationalParkDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult PostNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (_nationalParkRepository.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError("duplicates", "Park already exists in the database");
                return BadRequest(ModelState);
            }


            var nationalParkToPost = _mapper.Map<NationalPark>(nationalParkDto);

            if (!_nationalParkRepository.AddNationalParkToDatabase(nationalParkToPost))
            {
                ModelState.AddModelError(string.Empty, "Something went wrong while saving the park");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            nationalParkDto.Id = nationalParkToPost.Id;

            return CreatedAtAction("GetNationalParkById", new { nationalParkId = nationalParkToPost.Id },
                nationalParkDto);
        }

        /// <summary>
        ///     updates the national park with the given id to the new provided values
        /// </summary>
        /// <param name="nationalParkId"></param>
        /// <param name="nationalParkDto"></param>
        /// <returns></returns>
        [HttpPatch("{nationalParkId:int}", Name = "UpdateNationalPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateNationalPark(int nationalParkId, [FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto.Id != nationalParkId) return Unauthorized();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!_nationalParkRepository.NationalParkExists(nationalParkId))
            {
                ModelState.AddModelError("non-existing park", "Can't update a non-existing park");
                return BadRequest(ModelState);
            }

            var nationalPark = _mapper.Map<NationalPark>(nationalParkDto);

            if (_nationalParkRepository.UpdateNationalPark(nationalPark)) return NoContent();

            ModelState.AddModelError("server error", "something went wrong while updating");
            return StatusCode(500, ModelState);
        }

        /// <summary>
        ///     deletes the national park with the given id from the record of national parks
        /// </summary>
        /// <param name="nationalParkId"></param>
        /// <returns></returns>
        [HttpDelete("{nationalParkId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            if (!_nationalParkRepository.NationalParkExists(nationalParkId)) return BadRequest();

            if (!_nationalParkRepository.DeleteNationalParkFromDatabase(nationalParkId))
                return StatusCode(500, new { error = "something went wrong while deleting the park" });

            return NoContent();
        }
    }
}