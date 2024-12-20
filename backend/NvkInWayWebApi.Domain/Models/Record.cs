using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NvkInWayWebApi.Domain.Models.Profiles;

namespace NvkInWayWebApi.Domain.Models
{
    public class Record
    {
        public Guid Id { get; set; }

        public DriverProfile Driver { get; set; }

        public Trip Trip { get; set; }

        public PassengerProfile Passenger { get; set; }
    }
}
