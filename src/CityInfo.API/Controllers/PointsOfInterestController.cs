using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities/{cityId}/pointsofinterest")]
    public class PointsOfInterestController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            var city = CitiesDataStore.Current.Cities
                .SingleOrDefault(c => c.Id == cityId);

            if (city == null)
                return NotFound();

            return Ok(city.PointsOfInterest);
        }

        [HttpGet("{id}", Name = "GetPointOfInterestById")]
        public IActionResult GetPointOfInterestById(int cityId, int id)
        {
            var city = CitiesDataStore.Current.Cities
                .SingleOrDefault(c => c.Id == cityId);

            if (city == null)
                return NotFound();

            var pointOfInterest = city.PointsOfInterest
                .SingleOrDefault(p => p.Id == id);

            if (pointOfInterest == null)
                return NotFound();

            return Ok(pointOfInterest);
        }

        [HttpPost]
        public IActionResult CreatePointOfInterest(int cityId,
            [FromBody] PointOfInterestForCreationDto pointOfInterest)
        {
            if (pointOfInterest.Name == pointOfInterest.Description)
            {
                ModelState.AddModelError(
                    "Description",
                    "The description must be different from the name.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var city = CitiesDataStore.Current.Cities
                .SingleOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound("City does not exist");
            }
            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            var maxPointOfInterestId = CitiesDataStore.Current.Cities
                .SelectMany(c => c.PointsOfInterest)
                .Max(p => p.Id);

            var newPointOfInterest = new PointOfInterestDto()
            {
                Id = ++maxPointOfInterestId,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description

            };

            city.PointsOfInterest.Add(newPointOfInterest);

            return CreatedAtAction(
                "GetPointOfInterestById",
                new { cityId, id = newPointOfInterest.Id },
                newPointOfInterest);
        }
    }
}
