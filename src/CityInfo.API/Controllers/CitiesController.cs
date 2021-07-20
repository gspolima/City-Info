using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository repo;
        private readonly IMapper mapper;

        public CitiesController(ICityInfoRepository repo, IMapper mapper)
        {
            this.repo = repo ?? throw new
                ArgumentNullException(nameof(repo));
            this.mapper = mapper ?? throw new
                ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public IActionResult GetCities()
        {
            var cities = repo.GetCities();

            return Ok(mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cities));
        }

        [HttpGet("{id}", Name = "GetCityById")]
        public IActionResult GetCityById(int id, bool includePointsOfInterest = false)
        {
            var city = repo.GetCityById(id, includePointsOfInterest);

            if (city == null)
                return NotFound();

            if (includePointsOfInterest)
                return Ok(mapper.Map<CityDto>(city));

            return Ok(mapper.Map<CityWithoutPointsOfInterestDto>(city));
        }

        [HttpPost]
        public IActionResult AddNewCity([FromBody] CityForCreationDto city)
        {
            if (city == null)
                return BadRequest();

            var cityExists = repo.CityExists(city.Name);

            if (cityExists)
                return BadRequest("City exists already.");

            var newCity = new City()
            {
                Name = city.Name,
                Description = city.Description
            };

            repo.AddCity(newCity);

            var cityDto = new CityDto()
            {
                Id = newCity.Id,
                Name = newCity.Name,
                Description = newCity.Description
            };

            return CreatedAtAction("GetCityById", new { id = cityDto.Id, includePointsOfInterest = false }, cityDto);
        }
    }
}
