using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NvkInWayWebApi.Domain.Models.Profiles;

namespace NvkInWayWebApi.Domain.RepositoriesContract
{
    public interface IDriverRepository
    {
        public Task AddDriverAsync(DriverProfile driver);

        public Task UpdateDriverAsync(DriverProfile driver);

        public Task DeleteDriverAsync(Guid id);

        public Task GetDriverProfileById(Guid id);
    }
}
