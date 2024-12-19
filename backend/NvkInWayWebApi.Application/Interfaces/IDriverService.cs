using NvkInWayWebApi.Application.Common.Dtos.Driver.ReqDtos;
using NvkInWayWebApi.Application.Common.Dtos.Driver.ResDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NvkInWayWebApi.Domain;

namespace NvkInWayWebApi.Application.Interfaces
{
    public interface IDriverService
    {
        public Task<OperationResult<DriverProfileResDto>> GetDriverProfileByIdAsync(long profileId);

        public Task<OperationResult> AddDriverProfileAsync(DriverProfileReqDto driverProfileReqDto);
    }
}
