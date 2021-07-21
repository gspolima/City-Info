using CityInfo.API.Entities;
using System.Collections.Generic;

namespace CityInfo.API.Services
{
    public interface ICityInfoRepository
    {
        IEnumerable<City> GetCities();
        City GetCityById(int id);
        City GetCityById(int id, bool includePointsOfInterest);
        IEnumerable<PointOfInterest> GetPointsOfInterest(int cityId);
        PointOfInterest GetPointOfInterestById(int cityId, int id);
        int AddCity(City newCity);
        int AddPointOfInterest(int cityId, PointOfInterest newPoint);
        int UpdateCity(City city);
        int UpdatePointOfInterest(PointOfInterest pointOfInterest);
        int DeleteCity(City city);
        int DeletePointOfInterest(PointOfInterest pointOfInterest);
        bool CityExists(int cityId);
        bool CityExists(string name);
    }
}
