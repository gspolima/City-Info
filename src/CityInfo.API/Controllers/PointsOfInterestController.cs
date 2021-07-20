using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities/{cityId}/pointsofinterest")]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> logger;
        private readonly IMailService mailService;
        private readonly ICityInfoRepository repo;
        private readonly IMapper mapper;

        public PointsOfInterestController(
            ILogger<PointsOfInterestController> logger,
            IMailService mailService,
            ICityInfoRepository repo,
            IMapper mapper)
        {
            this.logger = logger ?? throw new
                ArgumentNullException(nameof(logger));
            this.mailService = mailService ?? throw new
                ArgumentNullException(nameof(mailService));
            this.repo = repo ?? throw new
                ArgumentNullException(nameof(repo));
            this.mapper = mapper ?? throw new
                ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            try
            {
                if (!IsExistingCity(cityId))
                {
                    logger.LogInformation($"The city ID {cityId} could not be found");
                    return NotFound();
                }

                var points = repo.GetPointsOfInterest(cityId);

                if (points.Count() == 0)
                    return NotFound("No points of interest found for this city");

                return Ok(mapper.Map<IEnumerable<PointOfInterestDto>>(points));
            }
            catch (Exception exception)
            {
                logger.LogCritical(
                    $"Exception thrown when getting the points of interest for city ID {cityId}. [{exception.Message}]");
                return StatusCode(500, "An error has ocurred when processing your request.");
            }

        }

        [HttpGet("{id}", Name = "GetPointOfInterestById")]
        public IActionResult GetPointOfInterestById(int cityId, int id)
        {
            if (!IsExistingCity(cityId))
                return NotFound();

            var point = repo.GetPointOfInterestById(cityId, id);

            if (point == null)
                return NotFound();

            return Ok(mapper.Map<PointOfInterestDto>(point));
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

            var success = city.PointsOfInterest.Remove(pointOfInterest);
            if (success)
            {
                mailService.Send(
                    "Point of Interest deleted",
                    "A point of interest was just deleted via API");
            }

            return NoContent();
        }

        public bool IsExistingCity(int cityId)
        {
            return repo.CityExists(cityId);
        }
    }
}
