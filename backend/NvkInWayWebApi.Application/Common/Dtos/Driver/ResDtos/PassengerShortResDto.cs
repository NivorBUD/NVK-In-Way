using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NvkInWayWebApi.Application.Common.Dtos.Driver.ResDtos
{
    public class PassengerShortResDto
    {
        public double Rating { get; set; }

        public int TripsCount { get; set; }

        public long TgProfileId { get; set; }
    }
}
