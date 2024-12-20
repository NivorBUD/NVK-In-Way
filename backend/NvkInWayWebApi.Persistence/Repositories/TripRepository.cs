using Microsoft.EntityFrameworkCore;
using NvkInWayWebApi.Domain;
using NvkInWayWebApi.Domain.Models;
using NvkInWayWebApi.Domain.RepositoriesContract;
using NvkInWayWebApi.Persistence.Entities;

namespace NvkInWayWebApi.Persistence.Repositories
{
    public class TripRepository : CommonRepository<TripEntity>, ITripRepository
    {
        public TripRepository(NvkInWayContext context) : base(context)
        {
        }

        public async Task<OperationResult> AddTripAsync(Trip trip)
        {
            await AddAsync(TripEntity.MapFrom(trip));
            await SaveChangesAsync();
            return OperationResult.Success(201);
        }

        public async Task<OperationResult> DeleteTripAsync(Guid id)
        {
            var trip = await _dbSet.FirstOrDefaultAsync(t => t.Id == id);
            if (trip != null)
            {
                Delete(trip);
                await SaveChangesAsync();
                return OperationResult.Success(204);
            }
            return OperationResult.Error("Поездка для удаления не обнаружен");
        }

        public async Task<OperationResult<Trip>> GetTripByTripIdAsync(Guid tripId)
        {
            var dbTrip = await _dbSet
                    .Include(d => d.Driver)
                    .Include(t => t.Car)
                    .Include(p => p.StartPointNavigation)
                    .Include(p => p.EndPointNavigation)
                    .Include(t => t.Records)
                    .FirstOrDefaultAsync(t => t.Id == tripId)
                ;

            if (dbTrip == null)
                return OperationResult<Trip>.Error("Поездка с таким идентификатором не была обнаружена");

            var trip = TripEntity.MapFrom(dbTrip);

            return OperationResult<Trip>.Success(trip);
        }

        public async Task<OperationResult<List<Trip>>> GetTripsByDriverIdAsync(long driverId)
        {
            var tripEntities = await _context.Set<TripEntity>()
                .Include(d => d.Driver)
                .Include(t => t.Car)
                .Include(p => p.StartPointNavigation)
                .Include(p => p.EndPointNavigation)
                .Include(t => t.Records)
                .Where(t => t.Driver.TgProfileId == driverId).ToListAsync();

            if (tripEntities == null)
                return OperationResult<List<Trip>>.Error("Поездки не найдены");

            var result = new List<Trip>();

            foreach (var tripEntity in tripEntities)
            {
                result.Add(TripEntity.MapFrom(tripEntity));
            }

            return OperationResult<List<Trip>>.Success(result);
        }

        public async Task<OperationResult<List<Trip>>> GetTripsByCarIdAsync(Guid carId)
        {
            var tripEntities = await _context.Set<TripEntity>()
                .Include(d => d.Driver)
                .Include(t => t.Car)
                .Include(t => t.StartPointNavigation)
                .Include(t => t.EndPointNavigation)
                .Include(t => t.Records)
                .Where(t => t.Car.Id == carId).ToListAsync();

            if (tripEntities == null)
                return OperationResult<List<Trip>>.Error("Поездки не найдены");

            var result = new List<Trip>();

            foreach (var tripEntity in tripEntities)
            {
                result.Add(TripEntity.MapFrom(tripEntity));
            }

            return OperationResult<List<Trip>>.Success(result);
        }

        public async Task<OperationResult> RecordToTripAsync(Record record)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var driverEntity = await _context.Set<DriverEntity>()
                    .Include(d => d.Trips)
                    .FirstOrDefaultAsync(d => d.TgProfileId == record.Driver.TgProfileId);

                if (driverEntity == null)
                    return OperationResult.Error("Водитель не найден");

                var passengerEntity = await _context.Set<PassengerEntity>()
                    .Include(p => p.Records)
                    .FirstOrDefaultAsync(d => d.TgProfileId == record.Passenger.TgProfileId);

                if (passengerEntity == null)
                    return OperationResult.Error("Пассажир не найден");

                var tripEntity = await _context.Set<TripEntity>()
                    .FirstOrDefaultAsync(d => d.Id == record.Trip.Id);

                if (tripEntity == null)
                    return OperationResult.Error("Поездка не найдена");

                await _context.Set<RecordEntity>().AddAsync(MapFrom(record));

                await _context.SaveChangesAsync();
                await transaction.CommitAsync(); // Завершение транзакции
            }
            catch (DbUpdateConcurrencyException ex)
            {
                await transaction.RollbackAsync(); // Откат транзакции
                return OperationResult.Error("Произошла ошибка при записи на поездку. Возможно, данные были изменены.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); // Откат транзакции
                return OperationResult.Error("Произошла ошибка: " + ex.Message);
            }

            return OperationResult.Success(204);
        }

        public Task<OperationResult> UpdateTripAsync(Trip trip)
        {
            throw new NotImplementedException();
        }

        public RecordEntity MapFrom(Record record)
        {
            var recordEntity = new RecordEntity
            {
                Id = record.Id,
                DriverId = record.Driver.TgProfileId,
                PassengerId = record.Passenger.TgProfileId,
                TripId = record.Trip.Id
            };

            return recordEntity;
        }
    }
}
