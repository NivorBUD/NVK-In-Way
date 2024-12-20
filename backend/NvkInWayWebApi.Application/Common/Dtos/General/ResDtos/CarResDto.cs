using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NvkInWayWebApi.Domain.Models;

namespace NvkInWayWebApi.Application.Common.Dtos.General.ResDtos
{
    public class CarResDto
    {
        public Guid AutoId { get; set; }

        public string AutoName { get; set; }

        public string AutoNumber { get; set; }

        public string AutoColor { get; set; }

        public static CarResDto MapFrom(Car car)
        {
            return new()
            {
                AutoId = car.Id,
                AutoName = car.Name,
                AutoNumber = car.Number,
                AutoColor = car.Color
            };
        }
    }
}
