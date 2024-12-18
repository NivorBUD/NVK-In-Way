using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NvkInWayWebApi.Application.Common.Dtos.Driver.ResDtos
{
    public class GetActiveTripsResDto
    {
        public LocationResDto CarLocation { get; set; }

        public LocationResDto StartPoint { get; set; }

        public LocationResDto EndPoint { get; set; }

        public DateTime TripStartTime { get; set; }

        public DateTime TripEndTime { get; set; }

        public int TotalPlaces { get; set; }

        public double DriveCost { get; set; }

        public int BookedPlaces { get; set; }

        public CarResDto TripCar { get; set; }
    }
}
