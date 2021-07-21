using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
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
        private readonly IMailService mailService;

        public CitiesController(ICityInfoRepository repo, IMapper mapper, IMailService mailService)
        {
            this.repo = repo ?? throw new
                ArgumentNullException(nameof(repo));
            this.mapper = mapper ?? throw new
                ArgumentNullException(nameof(mapper));
            this.mailService = mailService ?? throw new
                ArgumentNullException(nameof(mailService));
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

            if (repo.CityExists(city.Name))
                return BadRequest("City exists already.");

            var cityEntity = new City();

            mapper.Map(city, cityEntity);

            repo.AddCity(cityEntity);

            var createdCity = mapper.Map<CityWithoutPointsOfInterestDto>(cityEntity);

            return CreatedAtAction("GetCityById", new { id = createdCity.Id }, createdCity);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCity(int id,
            [FromBody] CityForUpdateDto city)
        {
            if (city == null)
                return BadRequest();

            if (city.Name == city.Description)
            {
                ModelState.AddModelError(
                    "Values",
                    "Name and Description must be different.");
            }

            if (string.IsNullOrEmpty(city.Name) || string.IsNullOrEmpty(city.Description))
            {
                ModelState.AddModelError(
                    "Values",
                    "Name and Description must have a non-empty value.");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cityEntity = repo.GetCityById(id);

            if (cityEntity == null)
                return NotFound();

            mapper.Map(city, cityEntity);

            repo.UpdateCity(cityEntity);

            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult PartialUpdateCity(int id,
            [FromBody] JsonPatchDocument<CityForUpdateDto> jsonPatchDoc)
        {
            if (jsonPatchDoc == null)
                return BadRequest();

            var cityEntity = repo.GetCityById(id);

            if (cityEntity == null)
                return NotFound();

            var cityToPatch = mapper.Map<CityForUpdateDto>(cityEntity);

            jsonPatchDoc.ApplyTo(cityToPatch, ModelState);

            if (!ModelState.IsValid)
                return BadRequest();

            if (cityToPatch.Name == cityToPatch.Description)
            {
                ModelState.AddModelError(
                    "Name",
                    "Name must be different from description");
            }

            if (!TryValidateModel(cityToPatch))
                return BadRequest(ModelState);

            mapper.Map(cityToPatch, cityEntity);

            repo.UpdateCity(cityEntity);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCity(int id)
        {
            var city = repo.GetCityById(id);

            if (city == null)
                return NotFound();

            var deletedRows = repo.DeleteCity(city);

            if (deletedRows == 1)
                mailService.Send(
                    "City deleted",
                    $"City {city.Name} with Id {city.Id} was just deleted.");

            return NoContent();
        }
    }
}
