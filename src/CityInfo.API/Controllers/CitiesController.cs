using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository repo;

        public CitiesController(ICityInfoRepository repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public IActionResult GetCities()
        {
            var cities = repo.GetCities();
            var citiesToReturn = new List<CityWithoutPointsOfInterest>();
            foreach (var city in cities)
            {
                citiesToReturn.Add(
                    new CityWithoutPointsOfInterest()
                    {
                        Id = city.Id,
                        Name = city.Name,
                        Description = city.Description
                    });
            }

            return Ok(citiesToReturn);
        }

        [HttpGet("{id}")]
        public IActionResult GetCityById(int id, bool includePointsOfInterest = false)
        {
            var city = repo.GetCityById(id, includePointsOfInterest);

            if (city == null)
                return NotFound();

            if (includePointsOfInterest)
            {
                var cityWithPoints = new CityDto()
                {
                    Id = city.Id,
                    Name = city.Name,
                    Description = city.Description
                };

                foreach (var point in city.PointsOfInterest)
                {
                    cityWithPoints.PointsOfInterest.Add(
                        new PointOfInterestDto()
                        {
                            Id = point.Id,
                            Name = point.Name,
                            Description = point.Description
                        });
                }

                return Ok(cityWithPoints);
            }

            var cityWithoutPoints = new CityWithoutPointsOfInterest()
            {
                Id = city.Id,
                Name = city.Name,
                Description = city.Description
            };

            return Ok(cityWithoutPoints);
        }
    }
}
