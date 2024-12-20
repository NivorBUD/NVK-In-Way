using NvkInWayWebApi.Application.Common.Dtos.CarTrip.ReqDtos;
using NvkInWayWebApi.Application.Common.Dtos.CarTrip.ResDtos;
using NvkInWayWebApi.Domain;
using NvkInWayWebApi.Domain.Models;

namespace NvkInWayWebApi.Application.Interfaces
{
    public interface ITripService
    {
        public Task<OperationResult<List<ShortActiveTripResDto>>> GetShortTripInfoByIntervalAsync(
            IntervalSearchReqDto searchDto, int startIndex, int count);

        public Task<OperationResult<List<GetActiveTripsResDto>>> GetTripsByDriverIdAsync(long driverId);

        public Task<OperationResult<GetActiveTripsResDto>> GetTripById(Guid id);

        public Task<OperationResult> AddDriverTripAsync(CreateTripReqDto tripReqDto);

        public Task<OperationResult> UpdateDriverTripAsync(CreateTripReqDto tripReqDto);

        public Task<OperationResult> DeleteTripAsync(Guid trip);
    }
}
