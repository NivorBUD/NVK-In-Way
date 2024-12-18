using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NvkInWayWebApi.Persistence.Entities;

namespace NvkInWayWebApi.Persistence.Repositories
{
    public class DriverRepository : CommonRepository<Driver>
    {
        public DriverRepository(DbContext context) : base(context) { }

        public async Task AddDriverAsync(Driver driver)
        {
            await AddAsync(driver);
            await SaveChangesAsync();
        }

        public async Task UpdateDriverAsync(Driver driver)
        {
            Update(driver);
            await SaveChangesAsync();
        }

        public async Task DeleteDriverAsync(string id)
        {
            var driver = await GetByIdAsync(id);
            if (driver != null)
            {
                Delete(driver);
                await SaveChangesAsync();
            }
        }
    }
}
