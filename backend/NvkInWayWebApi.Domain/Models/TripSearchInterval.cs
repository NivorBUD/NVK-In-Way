using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NvkInWayWebApi.Domain.Models;

namespace NvkInWayWebApi.Domain.Models
{
    public class TripSearchInterval
    {
        public Location? StartPointLocation { get; set; }

        public Location? EndPointLocation { get; set; }

        public DateTime? MaxEndTime { get; set; }

        public DateTime? MinDateTime { get; set; }
    }
}
