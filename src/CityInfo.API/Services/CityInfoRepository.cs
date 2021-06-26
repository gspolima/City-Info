using CityInfo.API.Contexts;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CityInfo.API.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityInfoContext context;
        public CityInfoRepository(CityInfoContext context)
        {
            this.context = context;
        }

        public IEnumerable<City> GetCities()
        {
            var cities = context.Cities
                .OrderBy(c => c.Name)
                .ToList();

            return cities;
        }

        public City GetCityById(int id, bool includePointsOfInterest)
        {
            City city;

            if (includePointsOfInterest)
            {
                city = context.Cities
                    .Include(c => c.PointsOfInterest)
                    .Where(c => c.Id == id)
                    .FirstOrDefault();
            }
            else
            {
                city = context.Cities
                    .Where(c => c.Id == id)
                    .FirstOrDefault();
            }

            return city;
        }

        public IEnumerable<PointOfInterest> GetPointsOfInterest(int cityId)
        {
            var pointsOfInterest = context.PointsOfInterest
                .Where(p => p.CityId == cityId)
                .OrderBy(p => p.Name)
                .ToList();

            return pointsOfInterest;
        }

        public PointOfInterest GetPointOfInterestById(int cityId, int id)
        {
            var pointOfInterest = context.PointsOfInterest
                .Where(p => p.CityId == cityId && p.Id == id)
                .FirstOrDefault();

            return pointOfInterest;
        }

        public bool CityExists(int cityId)
        {
            return context.Cities.Any(c => c.Id == cityId);
        }
    }
}
