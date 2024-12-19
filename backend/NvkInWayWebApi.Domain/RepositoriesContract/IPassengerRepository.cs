using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NvkInWayWebApi.Domain.Models.Profiles;

namespace NvkInWayWebApi.Domain.RepositoriesContract
{
    public interface IPassengerRepository
    {
        public Task<PassengerProfile?> GetPassengerWithRecordsAsync(Guid id);

        public Task AddPassengerAsync(PassengerProfile passenger);

        public Task UpdatePassengerAsync(PassengerProfile passenger);

        public Task DeletePassengerAsync(Guid id);
    }
}
