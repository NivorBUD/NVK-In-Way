using NvkInWayWebApi.Application.Common.Dtos.Driver.ResDtos;
using NvkInWayWebApi.Application.Common.Dtos.General.ReqDtos;
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
        public List<CarReqDto> Cars { get; set; }

        public long TgProfileId { get; set; }
    }
}
