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

        [HttpGet("{nationalParkId}")]
        public IActionResult GetNationalParkById(int nationalParkId)
        {
            var nationalPark = _nationalParkRepository.GetNationalParkById(nationalParkId);

            if (nationalPark == null)
            {
                return NotFound("There is no national park with that id");
            }
            else
            {
                var parkDto = _mapper.Map<NationalPark, NationalParkDto>(nationalPark);

                return Ok(parkDto);
            }
        }
    }
}
