using NvkInWayWebApi.Application.Common.Dtos.Driver.ReqDtos;
using NvkInWayWebApi.Application.Common.Dtos.Driver.ResDtos;
using NvkInWayWebApi.Domain;

namespace NvkInWayWebApi.Application.Interfaces
{
    public interface ITripService
    {
        public Task<OperationResult<GetActiveTripsResDto>> GetTripsByDriverIdAsync(long driverId);

        public Task<OperationResult> AddDriverTripAsync(CreateTripReqDto tripReqDto);

        public Task<OperationResult> UpdateDriverTripAsync(CreateTripReqDto tripReqDto);

        public Task<OperationResult> DeleteTripAsync(Guid trip);
    }
}
