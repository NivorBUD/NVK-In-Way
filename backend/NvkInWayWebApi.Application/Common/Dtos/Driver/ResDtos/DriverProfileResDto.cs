using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NvkInWayWebApi.Application.Common.Dtos.Driver;

namespace NvkInWayWebApi.Application.Common.Dtos.Driver.ResDtos
{
    public class DriverProfileResDto
    {
        public CarResDto Car { get; set; }

        public long TgProfileId { get; set; }

        public double Rating { get; set; }

        public int AllTripsCount { get; set; }
    }
}
