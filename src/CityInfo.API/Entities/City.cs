﻿using System.Collections.Generic;

namespace CityInfo.API.Entities
{
    public class City
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<PointOfInterest> PointsOfInterest { get; set; }
            = new List<PointOfInterest>();
    }
}
