using NvkInWayWebApi.Domain.Models;

namespace NvkInWayWebApi.Domain.RepositoriesContract
{
    public interface ITripRepository
    {
        public Task<OperationResult<List<Trip>>> GetTripsByDriverIdAsync(long driverId);

        public Task<OperationResult<List<Trip>>> GetTripsByCarIdAsync(Guid carId);

        public Task<OperationResult<Trip>> GetAllTripsAsync();

        public Task<OperationResult> AddTripAsync(Trip trip);

        public Task<OperationResult> UpdateTripAsync(Trip trip);

        public Task<OperationResult> DeleteTripAsync(Guid id);
    }
}
