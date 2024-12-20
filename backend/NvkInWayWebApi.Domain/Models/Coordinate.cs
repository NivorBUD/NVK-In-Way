using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NvkInWayWebApi.Domain.Models
{
    public class Coordinate
    {
        private const int MinLatitude = -90;
        private const int MaxLatitude = 90;
        private const int MinLongitude = -180;
        private const int MaxLongitude = 180;

        public double? Latitude { get; }
        public double? Longitude { get; }

        public Coordinate(double? latitude, double? longitude)
        {
            if (latitude < MinLatitude || latitude > MaxLatitude)
            {
                throw new ArgumentOutOfRangeException(nameof(latitude), $"Широта должна быть между {MinLatitude} и {MaxLatitude}.");
            }

            if (longitude < MinLongitude || longitude > MaxLongitude)
            {
                throw new ArgumentOutOfRangeException(nameof(longitude), $"Долгота должна быть между {MinLongitude} и {MaxLongitude}.");
            }

            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
