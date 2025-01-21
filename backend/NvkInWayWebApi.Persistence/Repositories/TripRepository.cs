using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NvkInWayWebApi.Domain;
using NvkInWayWebApi.Domain.Models;
using NvkInWayWebApi.Domain.Models.Profiles;
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

        public async Task<OperationResult<List<Trip>>> GetTripsByIntervalAsync(TripSearchInterval searchDto, int startIndex, int count)
        {
            // Начинаем формировать запрос
            var query = _dbSet.AsQueryable();

            // Применяем фильтры на основе входных параметров
            if (searchDto.StartPointLocation != null)
            {
                query = query.Where(trip => trip.StartPointNavigation.Description == LocationEntity.MapFrom(searchDto.StartPointLocation).Description);
            }

            if (searchDto.EndPointLocation != null)
            {
                query = query.Where(trip => trip.EndPointNavigation.Description == LocationEntity.MapFrom(searchDto.EndPointLocation).Description);
            }

            if (searchDto.MinDateTime.HasValue)
            {
                query = query.Where(trip => trip.DriveStartTime >= searchDto.MinDateTime.Value);
            }

            if (searchDto.MaxEndTime.HasValue)
            {
                query = query.Where(trip => trip.DriveEndTime <= searchDto.MaxEndTime.Value);
            }

            // Применяем пагинацию
            var result = await query
                .Include(d => d.Driver)
                .Include(t => t.Car)
                .Include(p => p.StartPointNavigation)
                .Include(p => p.EndPointNavigation)
                .Include(t => t.Records)
                .Skip(startIndex)
                .Take(count)
                .Select(c => TripEntity.MapFrom(c))
                .ToListAsync();

            return OperationResult<List<Trip>>.Success(result);
        }

        public async Task<OperationResult<Trip>> GetTripByTripIdAsync(Guid tripId)
        {
            var trips = await GetTripsByPredicateAsync(0, 1, t => t.Id == tripId);
            var trip = trips.FirstOrDefault();

            if (trip == null)
                return OperationResult<Trip>.Error("Поездка с таким идентификатором не была обнаружена");

            return OperationResult<Trip>.Success(trip);
        }

        public async Task<OperationResult<List<Trip>>> GetTripsByDriverIdAsync(long driverId, int startIndex, int count)
        {
            var passengerEntity = await _context.Set<DriverEntity>()
                .Include(p => p.Records)
                .FirstOrDefaultAsync(p => p.TgProfileId == driverId);

            if (passengerEntity == null)
                return OperationResult<List<Trip>>.Error("Пользователь с таким профилем не найден");

            var trips = await GetTripsByPredicateAsync(startIndex, count, t => t.Driver.TgProfileId == driverId);

            return OperationResult<List<Trip>>.Success(trips);
        }

        public async Task<OperationResult<List<Trip>>> GetTripsByPassengerIdAsync(long passengerId, int startIndex, int count)
        {
            var passengerEntity = await _context.Set<PassengerEntity>()
                .Include(p => p.Records)
                .FirstOrDefaultAsync(p => p.TgProfileId == passengerId);

            if (passengerEntity == null)
                return OperationResult<List<Trip>>.Error("Пользователь с таким профилем не найден");

            var trips = await GetTripsByPredicateAsync(
                startIndex, count, t => t.Records.Any(record => record.PassengerId == passengerId));

            return OperationResult<List<Trip>>.Success(trips);
        }

        public async Task<OperationResult<List<Trip>>> GetActiveTripsByDriverIdAsync(long driverId, int startIndex, int count)
        {
            var passengerEntity = await _context.Set<DriverEntity>()
                .Include(p => p.Records)
                .FirstOrDefaultAsync(p => p.TgProfileId == driverId);

            if (passengerEntity == null)
                return OperationResult<List<Trip>>.Error("Пользователь с таким профилем не найден");

            var trips = await GetTripsByPredicateAsync(startIndex, count,
                t => t.Driver.TgProfileId == driverId && t.DriveEndTime > DateTimeOffset.UtcNow);

            return OperationResult<List<Trip>>.Success(trips);
        }

        public async Task<OperationResult<List<Trip>>> GetActiveTripsByPassengerIdAsync(long passengerId, int startIndex, int count)
        {
            var passengerEntity = await _context.Set<PassengerEntity>()
                .Include(p => p.Records)
                .FirstOrDefaultAsync(p => p.TgProfileId == passengerId);

            if (passengerEntity == null)
                return OperationResult<List<Trip>>.Error("Пользователь с таким профилем не найден");

            var trips = await GetTripsByPredicateAsync(startIndex, count,
                t => t.Records.Any(record => record.PassengerId == passengerId) && t.DriveEndTime > DateTimeOffset.UtcNow);

            return OperationResult<List<Trip>>.Success(trips);
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

                if (tripEntity.BookedPlaces >= tripEntity.TotalPlaces)
                {
                    await transaction.RollbackAsync();
                    return OperationResult.Error("Все места уже забронированы");
                }
                tripEntity.BookedPlaces++;

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

        public async Task<OperationResult> CompleteTripAsync(Trip endTrip)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var trip = await _context.Set<TripEntity>()
                    .Include(t => t.Records)
                    .FirstOrDefaultAsync(t => t.Id == endTrip.Id);

                if (trip == null)
                    return OperationResult.Error("Поездка не найдена.");

                if (trip.DriverId != endTrip.DriverId)
                    return OperationResult.Error("Только водитель может завершить поездку.");

                var participantIds = trip.Records.Select(r => r.PassengerId).ToList();

                await _context.Set<PassengerEntity>()
                    .Where(p => participantIds.Contains(p.TgProfileId))
                    .ForEachAsync(p => p.TripsCount++);

                var driver = await _context.Set<DriverEntity>()
                    .FirstOrDefaultAsync(d => d.TgProfileId == trip.DriverId);
                if (driver != null)
                {
                    driver.TripsCount++;
                }

                trip.DriveEndTime = DateTime.UtcNow;

                await SaveChangesAsync();
                await transaction.CommitAsync();

                return OperationResult.Success(200);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return OperationResult.Error("Ошибка завершения поездки: " + ex.Message);
            }
        }

        public async Task<OperationResult> RateParticipantAsync(Guid tripId, long raterId, long targetId, float rating)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var trip = await _context.Set<TripEntity>()
                    .Include(t => t.Records)
                    .FirstOrDefaultAsync(t => t.Id == tripId);

                if (trip == null)
                    return OperationResult.Error("Поездка не найдена.");

                var isParticipant = trip.Records.Any(r => r.PassengerId == raterId) || trip.DriverId == raterId;
                if (!isParticipant)
                    return OperationResult.Error("Вы не являетесь участником этой поездки.");

                var hasTarget = trip.Records.Any(t => t.PassengerId == targetId) || trip.DriverId == targetId;
                if (!hasTarget)
                    return OperationResult.Error("Не найден цель оценки рейтинга");

                var isPassenger = trip.Records.Any(p => p.PassengerId == raterId);

                if (isPassenger)
                {
                    var passengerRaterId = await _context.Set<PassengerEntity>().FirstOrDefaultAsync(p => p.TgProfileId == raterId);
                    if (passengerRaterId == null)
                        return OperationResult.Error($"Пользователь с ID {raterId} не найден.");

                    var driverTargetId = await _context.Set<DriverEntity>().FirstOrDefaultAsync(p => p.TgProfileId == targetId);
                    if (driverTargetId == null)
                        return OperationResult.Error($"Пользователь с ID {targetId} не найден.");

                    driverTargetId.RatingCount++;
                    driverTargetId.TotalRating += rating;
                    driverTargetId.Rating = driverTargetId.TotalRating / driverTargetId.RatingCount;
                }
                else
                {
                    var driverTargetId = await _context.Set<DriverEntity>().FirstOrDefaultAsync(p => p.TgProfileId == raterId);
                    if (driverTargetId == null)
                        return OperationResult.Error($"Пользователь с ID {raterId} не найден.");

                    var passengerTagetId = await _context.Set<PassengerEntity>().FirstOrDefaultAsync(p => p.TgProfileId == targetId);
                    if (passengerTagetId == null)
                        return OperationResult.Error($"Пользователь с ID {targetId} не найден.");

                    passengerTagetId.RatingCount++;
                    passengerTagetId.TotalRating += rating;
                    passengerTagetId.Rating = passengerTagetId.TotalRating / passengerTagetId.RatingCount;
                }

                await SaveChangesAsync();
                await transaction.CommitAsync();

                return OperationResult.Success(200);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return OperationResult.Error("Ошибка сохранения оценок: " + ex.Message);
            }
        }

        public async Task<OperationResult<Dictionary<Trip, List<long>>>> GetNotifyingProfilesFromTrips(int startTripIndex, int tripCount)
        {
            var now = DateTimeOffset.UtcNow;
            var upcomingTime = now.AddMinutes(30);

            var tripsForNotify = await _dbSet.Where(t => !t.NotifyingProcessed && t.DriveStartTime <= upcomingTime)
                .Include(t => t.Driver)
                .Include(t => t.Records)
                .Include(t => t.StartPointNavigation)
                .Include(t => t.EndPointNavigation)
                .Include(t => t.Car)
                .OrderBy(t => t.DriveStartTime)
                .Skip(startTripIndex)
                .Take(tripCount)
                .ToListAsync();

            foreach (var tripEntity in tripsForNotify)
                tripEntity.NotifyingProcessed = true;

            var tripUsersDictionary = new Dictionary<Trip, List<long>>();

            foreach (var tripEntity in tripsForNotify)
            {
                var userIds = new List<long> { tripEntity.DriverId };
                userIds.AddRange(tripEntity.Records.Select(r => r.PassengerId));

                var domainTrip = TripEntity.MapFrom(tripEntity);
                tripUsersDictionary[domainTrip] = userIds;
            }

            await _context.SaveChangesAsync();
            return OperationResult<Dictionary<Trip, List<long>>>.Success(tripUsersDictionary);
        }

        public Task<OperationResult> UpdateTripAsync(Trip trip)
        {
            throw new NotImplementedException();
        }

        #region Helpers

        private async Task<List<Trip>> GetTripsByPredicateAsync(int startIndex, int count, Expression<Func<TripEntity, bool>> tripPredicate)
        {
            var trips = await _context.Set<TripEntity>()
                .Include(d => d.Driver)
                .Include(t => t.Car)
                .Include(p => p.StartPointNavigation)
                .Include(p => p.EndPointNavigation)
                .Include(t => t.Records)
                .Where(tripPredicate)
                .Skip(startIndex)
                .Take(count)
                .Select(c => TripEntity.MapFrom(c))
                .ToListAsync();

            return trips;
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

        #endregion
    }
}
