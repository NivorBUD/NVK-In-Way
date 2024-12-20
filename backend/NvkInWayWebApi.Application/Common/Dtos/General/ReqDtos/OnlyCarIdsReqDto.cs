using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NvkInWayWebApi.Domain.Models;

namespace NvkInWayWebApi.Application.Common.Dtos.General.ReqDtos
{
    public class OnlyCarIdsReqDto
    {
        public Guid Id { get; set; }

        public long DriverId { get; set; }

        public static Car MapFrom(OnlyCarIdsReqDto reqDto)
        {
            return new Car
            {
                Id = reqDto.Id,
                DriverId = reqDto.DriverId
            };
        }
    }
}
