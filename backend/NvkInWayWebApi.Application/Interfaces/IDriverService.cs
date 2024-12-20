using NvkInWayWebApi.Application.Common.Dtos.Driver.ReqDtos;
using NvkInWayWebApi.Application.Common.Dtos.Driver.ResDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NvkInWayWebApi.Domain;
using NvkInWayWebApi.Domain.Models;
using NvkInWayWebApi.Application.Common.Dtos.General.ReqDtos;

namespace NvkInWayWebApi.Application.Interfaces
{
    public interface IDriverService
    {
        public Task<OperationResult<DriverProfileResDto>> GetDriverProfileByIdAsync(long profileId);

        public Task<OperationResult> DeleteDriverProfileByIdAsync(long profileId);

        public Task<OperationResult> AddDriverProfileAsync(DriverProfileReqDto driverProfileReqDto);

        public Task<OperationResult> DeleteDriverCars(long profileId, List<Guid> carIds);

        public Task<OperationResult> AddDriverCars(long profileId, List<CarReqDto> cars);

        public Task<OperationResult> UpdateDriverCars(long profileId, List<DetailedСarReqDto> cars);
    }
}
