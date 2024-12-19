using NvkInWayWebApi.Application.Common.Dtos.Driver.ResDtos;
using NvkInWayWebApi.Domain.Models.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NvkInWayWebApi.Application.Common.Dtos.Driver.ReqDtos
{
    public class DriverProfileReqDto
    {
        public List<CarResDto> Cars { get; set; }

        public long TgProfileId { get; set; }

        public float? Rating { get; set; }

        public int AllTripsCount { get; set; }
    }
}
