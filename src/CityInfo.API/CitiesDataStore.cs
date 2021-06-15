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
                    Description = "A fairly well-known rock n roll band was born there.",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 1,
                            Name = "The Beatles Story",
                            Description = "The world’s largest permanent exhibition purely devoted to telling the story of The Beatles’ rise to fame."
                        },
                        new PointOfInterestDto()
                        {
                            Id = 2,
                            Name = "Sefton Park",
                            Description = "A magnificent 200-acre Park that looks like a natural landscape rather than a man-made park."
                        }
                    }
                },
                new CityDto()
                {
                    Id = 2,
                    Name = "Paris",
                    Description = "The city with a gigantic tower.",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 1,
                            Name = "Louvre Museum",
                            Description = "Landmark art museum with vast collection."
                        },
                        new PointOfInterestDto()
                        {
                            Id = 2,
                            Name = "Arc de Triomphe",
                            Description = "Triumphal arch and national monument."
                        }
                    }
                },
                new CityDto()
                {
                    Id = 3,
                    Name = "Antwerp",
                    Description = "The city with a cathedral that was never really finished.",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 1,
                            Name = "Grand Place",
                            Description = "The Grote Markt with its town hall and numerous guild houses is the heart of the old town."
                        },
                        new PointOfInterestDto()
                        {
                            Id = 2,
                            Name = "Cathedral of Our Lady",
                            Description = "Belgium's largest Gothic church."
                        }
                    }
                }
            };
        }
    }
}
