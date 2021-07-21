using AutoMapper;
using CityInfo.API.Entities;
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
            if (pointOfInterest == null)
                return BadRequest();

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

            var city = repo.CityExists(cityId);

            if (!city)
                return NotFound("City does not exist");

            var newPoint = mapper.Map<PointOfInterest>(pointOfInterest);

            repo.AddPointOfInterest(cityId, newPoint);

            var createdPoint = mapper.Map<PointOfInterestDto>(newPoint);

            return CreatedAtAction(
                "GetPointOfInterestById",
                new { cityId, id = createdPoint.Id },
                createdPoint);
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

            var city = repo.CityExists(cityId);

            if (!city)
                return NotFound("City does not exist");

            var pointOfInterestEntity = repo.GetPointOfInterestById(cityId, id);

            if (pointOfInterestEntity == null)
                return NotFound();

            mapper.Map(pointOfInterest, pointOfInterestEntity);

            repo.UpdatePointOfInterest(pointOfInterestEntity);

            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult PatchPointOfInterest(int cityId, int id,
            JsonPatchDocument<PointOfInterestForUpdateDto> jsonPatchDoc)
        {
            var city = repo.CityExists(cityId);

            if (!city)
                return NotFound("City does not exist");

            var pointOfInterestEntity = repo.GetPointOfInterestById(cityId, id);

            if (pointOfInterestEntity == null)
                return NotFound();

            var pointOfInterestToPatch = mapper
                .Map<PointOfInterestForUpdateDto>(pointOfInterestEntity);

            jsonPatchDoc.ApplyTo(pointOfInterestToPatch, ModelState);

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

            mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);

            repo.UpdatePointOfInterest(pointOfInterestEntity);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            var city = repo.CityExists(cityId);

            if (!city)
                return NotFound("City does not exist");

            var pointOfInterest = repo.GetPointOfInterestById(cityId, id);

            if (pointOfInterest == null)
                return NotFound();

            var deletedRows = repo.DeletePointOfInterest(pointOfInterest);
            if (deletedRows == 1)
            {
                mailService.Send(
                    "Point of Interest deleted",
                    $"Point of interest {pointOfInterest.Name} with ID " +
                    $"{pointOfInterest.Id} was just deleted via API");
            }

            return NoContent();
        }

        public bool IsExistingCity(int cityId)
        {
            return repo.CityExists(cityId);
        }
    }
}
