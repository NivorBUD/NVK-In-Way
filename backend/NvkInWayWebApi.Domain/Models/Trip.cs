using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NvkInWayWebApi.Domain.Models
{
    public class Trip
    {
        public Guid Id { get; set; }

        public Guid DriverId { get; set; }

        public Location StartPoint { get; set; }

        public Location EndPoint { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int TotalPlaces { get; set; }

        public int BookedPlaces { get; set; }

        public double Cost { get; set; }

        public string CarLocation { get; set; }

        public Car DriverCar { get; set; }
    }
}
