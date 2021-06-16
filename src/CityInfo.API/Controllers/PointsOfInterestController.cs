using CityInfo.API.Models;
using Microsoft.AspNetCore.JsonPatch;
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
                return NotFound("City does not exist");

            return Ok(city.PointsOfInterest);
        }

        [HttpGet("{id}", Name = "GetPointOfInterestById")]
        public IActionResult GetPointOfInterestById(int cityId, int id)
        {
            var city = CitiesDataStore.Current.Cities
                .SingleOrDefault(c => c.Id == cityId);

            if (city == null)
                return NotFound("City does not exist");

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
                return NotFound("City does not exist");
            if (pointOfInterest == null)
                return BadRequest();

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

        [HttpPut("{id}")]
        public IActionResult UpdateWholePointOfInterest(int cityId, int id,
            [FromBody] PointOfInterestForUpdateDto pointOfInterest)
        {
            if (pointOfInterest.Name == pointOfInterest.Description)
            {
                ModelState.AddModelError(
                    "Description",
                    "The description must be different from the name.");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var city = CitiesDataStore.Current.Cities
                .SingleOrDefault(c => c.Id == cityId);

            if (city == null)
                return NotFound("City does not exist");

            var pointOfInterestFromStore = city.PointsOfInterest
                .SingleOrDefault(p => p.Id == id);

            if (pointOfInterestFromStore == null)
                return NotFound();

            pointOfInterestFromStore.Name = pointOfInterest.Name;
            pointOfInterestFromStore.Description = pointOfInterest.Description;

            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult PatchPointOfInterest(int cityId, int id,
            JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            var city = CitiesDataStore.Current.Cities
                .SingleOrDefault(c => c.Id == cityId);

            if (city == null)
                return NotFound("City does not exist");

            var pointOfInterestFromStore = city.PointsOfInterest
                .SingleOrDefault(p => p.Id == id);

            if (pointOfInterestFromStore == null)
                return NotFound();

            var pointOfInterestToPatch = new PointOfInterestForUpdateDto()
            {
                Name = pointOfInterestFromStore.Name,
                Description = pointOfInterestFromStore.Description
            };

            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
                return BadRequest();

            if (pointOfInterestToPatch.Name == pointOfInterestToPatch.Description)
            {
                ModelState.AddModelError(
                    "Description",
                    "The description must be different from the name.");
            }

            if (!TryValidateModel(pointOfInterestToPatch))
                return BadRequest(ModelState);

            pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
            pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            var city = CitiesDataStore.Current.Cities
                .SingleOrDefault(c => c.Id == cityId);

            if (city == null)
                return NotFound("City does not exist");

            var pointOfInterest = city.PointsOfInterest
                .SingleOrDefault(p => p.Id == id);

            if (pointOfInterest == null)
                return NotFound();

            city.PointsOfInterest.Remove(pointOfInterest);

            return NoContent();
        }
    }
}
