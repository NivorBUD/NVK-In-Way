using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NvkInWayWebApi.Domain.Models;

namespace NvkInWayWebApi.Application.Common.Dtos.Driver
{
    public class CarResDto
    {
        public string AutoName { get; set; }

        public string AutoNumber { get; set; }

        public string AutoColor { get; set; }

        public static CarResDto MapFrom(Car car)
        {
            return new()
            {
                AutoName = car.Name,
                AutoNumber = car.Number,
                AutoColor = car.Color
            };
        }
    }
}
