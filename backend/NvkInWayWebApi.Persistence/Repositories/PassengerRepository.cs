using Microsoft.EntityFrameworkCore;
using NvkInWayWebApi.Domain.Models.Profiles;
using NvkInWayWebApi.Domain.RepositoriesContract;
using NvkInWayWebApi.Persistence.Entities;

namespace NvkInWayWebApi.Persistence.Repositories
{
    public class PassengerRepository : CommonRepository<PassengerEntity>, IPassengerRepository
    {
        public PassengerRepository(NvkInWayContext context) : base(context) { }

        public async Task<PassengerProfile?> GetPassengerWithRecordsAsync(Guid id)
        {
            var passangerEntity = await _context.Set<PassengerEntity>()
                .Include(p => p.Records)
                .FirstOrDefaultAsync(p => p.Id == id);

            return MapFrom(passangerEntity);
        }

        public async Task AddPassengerAsync(PassengerProfile passenger)
        {
            await AddAsync(MapFrom(passenger));
            await SaveChangesAsync();
        }

        public async Task UpdatePassengerAsync(PassengerProfile passenger)
        {
            Update(MapFrom(passenger));
            await SaveChangesAsync();
        }

        public async Task DeletePassengerAsync(Guid id)
        {
            var passenger = await GetByIdAsync(id);
            if (passenger != null)
            {
                Delete(passenger);
                await SaveChangesAsync();
            }
        }

        public PassengerEntity MapFrom(PassengerProfile passengerProfile)
        {
            return new PassengerEntity
            {
                Id = passengerProfile.Id,
                TgProfileId = passengerProfile.TgProfileId,
                TripCount = passengerProfile.TripsCount,
                Rating = passengerProfile.Rating,
            };
        }

        public PassengerProfile MapFrom(PassengerEntity passengerProfile)
        {
            return new PassengerProfile
            {
                Id = passengerProfile.Id,
                TgProfileId = passengerProfile.TgProfileId,
                TripsCount = passengerProfile.TripCount,
                Rating = passengerProfile.Rating,
            };
        }
    }
}
