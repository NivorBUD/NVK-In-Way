using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NvkInWayWebApi.Domain.Models;

namespace NvkInWayWebApi.Application.Common.Dtos.General.ReqDtos
{
    public class DetailedСarReqDto
    {
        public Guid Id { get; set; }

        public string AutoName { get; set; }

        public string AutoNumber { get; set; }

        public string AutoColor { get; set; }

        public static Car MapFrom(DetailedСarReqDto detailed)
        {
            return new Car
            {
                Id = detailed.Id,
                Name = detailed.AutoName,
                Color = detailed.AutoColor,
                Number = detailed.AutoNumber
            };
        }
    }
}
