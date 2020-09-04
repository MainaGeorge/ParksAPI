using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.DTOs;
using ParkyAPI.Services.IRepositoryService;
using System.Collections.Generic;

namespace ParkyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NationalParksController : ControllerBase
    {
        private readonly INationalParkRepository _nationalParkRepository;
        private readonly IMapper _mapper;

        public NationalParksController(INationalParkRepository nationalParkRepository,
            IMapper mapper)
        {
            _nationalParkRepository = nationalParkRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllNationalParks()
        {
            var nationalParks = _nationalParkRepository.GetAllNationalParks();

            var nationalParksToReturn =
                _mapper.Map<IEnumerable<NationalPark>, IEnumerable<NationalParkDto>>(nationalParks);

            return Ok(nationalParksToReturn);
        }

        [HttpGet("{nationalParkId:int}", Name = "GetNationalParkById")]
        public IActionResult GetNationalParkById(int nationalParkId)
        {
            var nationalPark = _nationalParkRepository.GetNationalParkById(nationalParkId);

            if (nationalPark == null)
            {
                return NotFound();
            }
            else
            {
                var parkDto = _mapper.Map<NationalParkDto>(nationalPark);

                return Ok(parkDto);
            }
        }

        [HttpPost]
        public IActionResult PostNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
           
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_nationalParkRepository.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError("Can not allow duplicates", "Park already exists in the database");
                return BadRequest(ModelState);
            }

            

            var nationalParkToPost = _mapper.Map<NationalPark>(nationalParkDto);

            if (!_nationalParkRepository.AddNationalParkToDatabase(nationalParkToPost))
            {
                ModelState.AddModelError(string.Empty, "Something went wrong while saving the park");
                return StatusCode(500, ModelState);
            }

            nationalParkDto.Id = nationalParkToPost.Id;

            return CreatedAtAction("GetNationalParkById", new { nationalParkId = nationalParkToPost.Id }, nationalParkDto);
        }
    }
}
