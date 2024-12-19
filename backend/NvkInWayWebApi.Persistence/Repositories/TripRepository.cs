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
            await AddAsync(MapFrom(trip));
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

        public async Task<OperationResult<Trip>> GetAllTripsAsync()
        {
            await GetAllAsync();
            return OperationResult<Trip>.Success();
        }

        public async Task<OperationResult<List<Trip>>> GetTripsByDriverIdAsync(long driverId)
        {
            var tripEntities = await _context.Set<TripEntity>()
                .Include(t => t.Records)
                .Where(t => t.DriverId == driverId).ToListAsync();

            if (tripEntities == null)
                return OperationResult<List<Trip>>.Error("Поездки не найдены");

            var result = new List<Trip>();

            foreach (var tripEntity in tripEntities)
            {
                result.Add(MapFrom(tripEntity));
            }

            return OperationResult<List<Trip>>.Success(result);
        }

        public async Task<OperationResult<List<Trip>>> GetTripsByCarIdAsync(Guid carId)
        {
            var tripEntities = await _context.Set<TripEntity>()
                .Include(t => t.Records)
                .Where(t => t.CarId == carId).ToListAsync();

            if (tripEntities == null)
                return OperationResult<List<Trip>>.Error("Поездки не найдены");

            var result = new List<Trip>();

            foreach (var tripEntity in tripEntities)
            {
                result.Add(MapFrom(tripEntity));
            }

            return OperationResult<List<Trip>>.Success(result);
        }

        public Task<OperationResult> UpdateTripAsync(Trip trip)
        {
            throw new NotImplementedException();
        }

        public TripEntity MapFrom(Trip trip)
        {
            return new TripEntity()
            {
                Id = trip.Id,
                DriverId = trip.DriverId,
                CarId = trip.DriverCar.Id,
                StartPoint = trip.StartPoint,
                EndPoint = trip.EndPoint,
                DriveStartTime = trip.StartTime,
                DriveEndTime = trip.EndTime,
                TotalPlaces = trip.TotalPlaces,
                BookedPlaces = trip.BookedPlaces
            };
        }

        public Trip MapFrom(TripEntity trip)
        {
            // Добавить машину
            return new Trip()
            {
                Id = trip.Id,
                DriverId = trip.DriverId,
                // DriverCar = trip.Car,
                StartPoint = trip.StartPoint,
                EndPoint = trip.EndPoint,
                StartTime = trip.DriveStartTime,
                EndTime = trip.DriveEndTime,
                TotalPlaces = trip.TotalPlaces,
                BookedPlaces = trip.BookedPlaces
            };
        }
    }
}
