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

        [HttpGet("{id}")]
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
    }
}
