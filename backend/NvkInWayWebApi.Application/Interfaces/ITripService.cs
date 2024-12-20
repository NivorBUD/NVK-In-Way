using NvkInWayWebApi.Application.Common.Dtos.CarTrip;
using NvkInWayWebApi.Domain;
using NvkInWayWebApi.Domain.Models;

namespace NvkInWayWebApi.Application.Interfaces
{
    public interface ITripService
    {
        public Task<OperationResult<List<GetActiveTripsResDto>>> GetTripsByDriverIdAsync(long driverId);

        public Task<OperationResult<GetActiveTripsResDto>> GetTripById(Guid id);

        public Task<OperationResult> AddDriverTripAsync(CreateTripReqDto tripReqDto);

        public Task<OperationResult> UpdateDriverTripAsync(CreateTripReqDto tripReqDto);

        public Task<OperationResult> DeleteTripAsync(Guid trip);
    }
}
