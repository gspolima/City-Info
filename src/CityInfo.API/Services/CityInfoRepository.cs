using CityInfo.API.Contexts;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CityInfo.API.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityInfoContext context;
        public CityInfoRepository(CityInfoContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
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
        public int AddCity(City newCity)
        {
            context.Add(newCity);
            var affectedRows = context.SaveChanges();
            return affectedRows;
        }

        public int AddPointOfInterest(int cityId, PointOfInterest newPoint)
        {
            var city = GetCityById(cityId, includePointsOfInterest: false);

            city.PointsOfInterest.Add(newPoint);
            var affectedRows = context.SaveChanges();
            return affectedRows;
        }

        public bool CityExists(int cityId)
        {
            return context.Cities.Any(c => c.Id == cityId);
        }

        public bool CityExists(string name)
        {
            var exists = context.Cities
                .Any(c =>
            c.Name.ToUpperInvariant() == name.ToUpperInvariant());

            return exists;
        }

        public int UpdatePointOfInterest(PointOfInterest pointOfInterest)
        {
            context.Entry(pointOfInterest).State = EntityState.Modified;
            var affectedRows = context.SaveChanges();
            return affectedRows;
        }
    }
}
