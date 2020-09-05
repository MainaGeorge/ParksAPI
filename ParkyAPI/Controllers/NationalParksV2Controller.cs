using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.DTOs;
using ParkyAPI.Services.IRepositoryService;
using System.Collections.Generic;

namespace ParkyAPI.Controllers
{
    [Route("api/v{version:apiVersion}/nationalparks")]
    [ApiVersion("2.0")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class NationalParksV2Controller : ControllerBase
    {
        private readonly INationalParkRepository _nationalParkRepository;
        private readonly IMapper _mapper;

        public NationalParksV2Controller(INationalParkRepository nationalParkRepository, IMapper mapper)
        {
            _nationalParkRepository = nationalParkRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// returns a list of all the national parks available
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<NationalParkDto>))]
        public IActionResult GetAll()
        {
            var parks = _nationalParkRepository.GetAllNationalParks();

            return Ok(_mapper.Map<IEnumerable<NationalPark>, IEnumerable<NationalParkDto>>(parks));
        }


        /// <summary>
        ///     Returns a national park with the specified id or null if none is found
        /// </summary>
        /// <param name="nationalParkId"></param>
        /// <returns></returns>
        /// 
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
    }
}
