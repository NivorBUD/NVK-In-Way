using Microsoft.EntityFrameworkCore;
using NvkInWayWebApi.Domain;
using NvkInWayWebApi.Domain.Models.Profiles;
using NvkInWayWebApi.Domain.RepositoriesContract;
using NvkInWayWebApi.Persistence.Entities;

namespace NvkInWayWebApi.Persistence.Repositories
{
    public class PassengerRepository : CommonRepository<PassengerEntity>, IPassengerRepository
    {
        public PassengerRepository(NvkInWayContext context) : base(context) { }

        public async Task<OperationResult<PassengerProfile>> GetPassengerWithRecordsAsync(long profileId)
        {
            var passengerEntity = await _context.Set<PassengerEntity>()
                .Include(p => p.Records)
                .FirstOrDefaultAsync(p => p.TgProfileId == profileId);

            if (passengerEntity == null)
                return OperationResult<PassengerProfile>.Error("Пользователь с таким профилем не найден");

            var profile = MapFrom(passengerEntity);

            return OperationResult<PassengerProfile>.Success(profile);
        }

        public async Task<OperationResult> AddPassengerAsync(PassengerProfile passenger)
        {
            await AddAsync(MapFrom(passenger));
            await SaveChangesAsync();
            return OperationResult.Success(201);
        }

        public async Task<OperationResult> UpdatePassengerAsync(PassengerProfile passenger)
        {
            // Получаем существующего пассажира
            var existingPassenger = await _dbSet
                .FirstOrDefaultAsync(p => p.TgProfileId == passenger.TgProfileId);

            if (existingPassenger == null)
            {
                return OperationResult.Error("Пассажир не найден");
            }

            existingPassenger.Rating = passenger.Rating;
            existingPassenger.TripsCount = passenger.TripsCount;

            await SaveChangesAsync();
            return OperationResult.Success(201);
        }


        public async Task<OperationResult> DeletePassengerAsync(long profileId)
        {
            var passenger = await _dbSet.FirstOrDefaultAsync(d => d.TgProfileId == profileId);
            if (passenger != null)
            {
                Delete(passenger);
                await SaveChangesAsync();
                return OperationResult.Success(204);
            }
            return OperationResult.Error("Профиль для удаления не обнаружен");
        }

        public PassengerEntity MapFrom(PassengerProfile passengerProfile)
        {
            return new PassengerEntity
            {
                TgProfileId = passengerProfile.TgProfileId,
                TripsCount = passengerProfile.TripsCount,
                Rating = passengerProfile.Rating,
            };
        }

        public PassengerProfile MapFrom(PassengerEntity passengerProfile)
        {
            return new PassengerProfile
            {
                TgProfileId = passengerProfile.TgProfileId,
                TripsCount = passengerProfile.TripsCount,
                Rating = passengerProfile.Rating,
            };
        }
    }
}
