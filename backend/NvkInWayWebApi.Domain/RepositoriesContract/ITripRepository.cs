using NvkInWayWebApi.Domain.Models;

namespace NvkInWayWebApi.Domain.RepositoriesContract
{
    public interface ITripRepository
    {
        public Task<OperationResult<Trip>> GetTripByDriverIdAsync(Guid driverId);

        public Task<OperationResult<Trip>> GetTripByPassengerIdAsync(Guid passengerId);

        public Task<OperationResult<Trip>> GetAllTripsAsync();

        public Task<OperationResult> AddTripAsync(Trip trip);

        public Task<OperationResult> UpdateTripAsync(Trip trip);

        public Task<OperationResult> DeleteTripAsync(Guid id);
    }
}
