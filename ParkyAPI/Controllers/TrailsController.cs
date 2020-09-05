using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.DTOs;
using ParkyAPI.Services.IRepositoryService;
using System.Collections.Generic;

namespace ParkyAPI.Controllers
{
    [Route("api/[controller]")]
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TrailDto>))]
        public IActionResult GetAllTrails()
        {
            var trails = _trailRepository.GetAllTrails();

            var trailDtos = _mapper.Map<IEnumerable<Trail>, IEnumerable<TrailDto>>(trails);

            return Ok(trailDtos);
        }

        [HttpGet("{trailId:int}", Name = "GetTrailById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetTrailById(int trailId)
        {
            if (!_trailRepository.TrailExists(trailId)) return NotFound();

            var trail = _trailRepository.GetTrailById(trailId);

            var trailDto = _mapper.Map<Trail, TrailDto>(trail);

            return Ok(trailDto);

        }
    }
}
