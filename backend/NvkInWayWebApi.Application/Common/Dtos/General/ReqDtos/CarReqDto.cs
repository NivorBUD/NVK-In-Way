using NvkInWayWebApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NvkInWayWebApi.Application.Common.Dtos.General.ReqDtos
{
    public class CarReqDto
    {
        public string AutoName { get; set; }

        public string AutoNumber { get; set; }

        public string AutoColor { get; set; }

        public static Car MapFrom(CarReqDto carReqDto)
        {
            return new Car()
            {
                Name = carReqDto.AutoName,
                Color = carReqDto.AutoColor,
                Number = carReqDto.AutoNumber
            };
        }
    }
}
