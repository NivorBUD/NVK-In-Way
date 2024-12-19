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
        public Task<OperationResult> AddDriverAsync(DriverProfile driver);

        public Task<OperationResult> UpdateDriverAsync(DriverProfile driver);

        public Task<OperationResult> DeleteDriverAsync(long profileId);

        public Task<OperationResult<DriverProfile>> GetDriverProfileByIdAsync(long profileId);
    }
}
