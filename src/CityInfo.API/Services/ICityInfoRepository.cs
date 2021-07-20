using CityInfo.API.Entities;
using System.Collections.Generic;

namespace CityInfo.API.Services
{
    public interface ICityInfoRepository
    {
        IEnumerable<City> GetCities();
        City GetCityById(int id, bool includePointsOfInterest);
        int AddCity(City newCity);
        IEnumerable<PointOfInterest> GetPointsOfInterest(int cityId);
        PointOfInterest GetPointOfInterestById(int cityId, int id);
        int AddPointOfInterest(int cityId, PointOfInterest newPoint);
        int UpdatePointOfInterest(PointOfInterest pointOfInterest);
        bool CityExists(int cityId);
        bool CityExists(string name);
    }
}
