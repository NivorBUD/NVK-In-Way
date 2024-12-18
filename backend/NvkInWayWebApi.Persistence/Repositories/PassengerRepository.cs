using Microsoft.EntityFrameworkCore;
using NvkInWayWebApi.Persistence.Entities;

namespace NvkInWayWebApi.Persistence.Repositories
{
    public class PassengerRepository : CommonRepository<Passenger>
    {
        public PassengerRepository(NvkInWayContext context) : base(context) { }

        public async Task<Passenger?> GetPassengerWithRecordsAsync(string id)
        {
            return await _context.Set<Passenger>()
                .Include(p => p.Records)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddPassengerAsync(Passenger passenger)
        {
            await AddAsync(passenger);
            await SaveChangesAsync();
        }

        public async Task UpdatePassengerAsync(Passenger passenger)
        {
            Update(passenger);
            await SaveChangesAsync();
        }

        public async Task DeletePassengerAsync(string id)
        {
            var passenger = await GetByIdAsync(id);
            if (passenger != null)
            {
                Delete(passenger);
                await SaveChangesAsync();
            }
        }
    }
}
