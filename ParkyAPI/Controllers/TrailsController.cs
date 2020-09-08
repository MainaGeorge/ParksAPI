using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.DTOs;
using ParkyAPI.Services.IRepositoryService;
using System;
using System.Collections.Generic;

namespace ParkyAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/v{version:apiVersion}/Trails")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class TrailsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITrailRepository _trailRepository;

        public TrailsController(IMapper mapper, ITrailRepository trailRepository)
        {
            _mapper = mapper;
            _trailRepository = trailRepository;
        }

        /// <summary>
        /// Returns a collection of all the trails available in the records
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TrailDto>))]
        public IActionResult GetAllTrails()
        {
            var trails = _trailRepository.GetAllTrails();

            var trailDtos = _mapper.Map<IEnumerable<Trail>, IEnumerable<TrailDto>>(trails);

            return Ok(trailDtos);
        }

        /// <summary>
        /// returns the trail with the given trail id
        /// </summary>
        /// <param name="trailId"></param>
        /// <returns></returns>
        [HttpGet("{trailId:int}", Name = "GetTrailById")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetTrailById(int trailId)
        {
            if (!_trailRepository.TrailExists(trailId)) return NotFound();

            var trail = _trailRepository.GetTrailById(trailId);

            var trailDto = _mapper.Map<Trail, TrailDto>(trail);

            return Ok(trailDto);

        }
        /// <summary>
        /// returns all the trails in a given national park
        /// </summary>
        /// <param name="nationalParkId"></param>
        /// <returns></returns>
        [HttpGet("[action]/{nationalParkId:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TrailDto>))]
        public IActionResult GetAllTrailsInANationalPark(int nationalParkId)
        {
            var trails = _trailRepository.GetAllTrailsInANationalPark(nationalParkId);

            var trailsDto = _mapper.Map<IEnumerable<Trail>, IEnumerable<TrailDto>>(trails);

            return Ok(trailsDto);
        }

        /// <summary>
        /// Saves a trail to the records
        /// </summary>
        /// <param name="createTrailDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateTrailDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult PostTrail([FromBody] CreateTrailDto createTrailDto)
        {
            if (!ModelState.IsValid) return BadRequest(modelState: ModelState);

            if (_trailRepository.TrailExists(createTrailDto.Name))
            {
                ModelState.AddModelError("no duplicates", "Trail already exists");
                return BadRequest(ModelState);
            }

            var trailToPost = _mapper.Map<Trail>(createTrailDto);
            trailToPost.DateCreated = DateTime.Now;

            if (_trailRepository.PostNewTrail(trailToPost))
            {
                return CreatedAtAction("GetTrailById",
                    new { trailId = trailToPost.Id },
                    _mapper.Map<CreateTrailDto>(trailToPost));
            }

            ModelState.AddModelError("server error", "something went wrong while saving the trail");
            return StatusCode(StatusCodes.Status500InternalServerError, ModelState);

        }

        /// <summary>
        /// Removes a trail from the collection of stored trails
        /// </summary>
        /// <param name="trailId"></param>
        /// <returns></returns>
        [HttpDelete("{trailId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteTrail(int trailId)
        {
            if (!_trailRepository.TrailExists(trailId)) return BadRequest();

            if (_trailRepository.DeleteTrail(trailId)) return NoContent();

            ModelState.AddModelError("server error", "something went wrong while deleting");
            return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
        }

        /// <summary>
        /// updates the trail with the given trail id with the data provided
        /// </summary>
        /// <param name="trailId"></param>
        /// <param name="updateTrailDto"></param>
        /// <returns></returns>
        [HttpPatch("{trailId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult UpdateTrail(int trailId, UpdateTrailDto updateTrailDto)
        {
            if (trailId != updateTrailDto.Id) return Unauthorized();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!_trailRepository.TrailExists(trailId)) return BadRequest();

            if (_trailRepository.UpdateTrial(trailId, updateTrailDto)) return NoContent();

            ModelState.AddModelError("server error", "something went wrong while updating");
            return StatusCode(StatusCodes.Status500InternalServerError, ModelState);

        }
    }
}
