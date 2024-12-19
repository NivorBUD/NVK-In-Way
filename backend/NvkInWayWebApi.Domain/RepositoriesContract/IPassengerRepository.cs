using NvkInWayWebApi.Domain.Models.Profiles;

namespace NvkInWayWebApi.Domain.RepositoriesContract
{
    public interface IPassengerRepository
    {
        public Task<OperationResult<PassengerProfile>> GetPassengerWithRecordsAsync(long id);

        public Task<OperationResult> AddPassengerAsync(PassengerProfile passenger);

        public Task<OperationResult> UpdatePassengerAsync(PassengerProfile passenger);

        public Task<OperationResult> DeletePassengerAsync(long id);
    }
}
