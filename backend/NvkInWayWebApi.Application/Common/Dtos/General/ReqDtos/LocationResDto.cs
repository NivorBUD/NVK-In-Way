using NvkInWayWebApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NvkInWayWebApi.Application.Common.Dtos.General.ReqDtos
{
    public class LocationResDto
    {
        public string TextDescription { get; set; }

        public Coordinate Coordinate { get; set; }

        public static LocationResDto MapFrom(Location location)
        {
            return new LocationResDto()
            {
                TextDescription = location.TextDescription,
                Coordinate = location.Coordinate,
            };
        }
    }
}
