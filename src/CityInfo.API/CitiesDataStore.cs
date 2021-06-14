using CityInfo.API.Models;
using System.Collections.Generic;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        public static CitiesDataStore Current => new CitiesDataStore();
        public List<CityDto> Cities { get; set; }

        public CitiesDataStore()
        {
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id = 1,
                    Name = "Liverpool",
                    Description = "A fairly well-known rock n roll band was born there"
                },
                new CityDto()
                {
                    Id = 2,
                    Name = "Paris",
                    Description = "The city with a gigantic tower."
                },
                new CityDto()
                {
                    Id = 3,
                    Name = "Antwerp",
                    Description = "The city with a cathedral that was never really finished"
                }
            };
        }
    }
}
