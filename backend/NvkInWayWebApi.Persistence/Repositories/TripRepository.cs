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
                result.Add(TripEntity.MapFrom(tripEntity));
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
                result.Add(TripEntity.MapFrom(tripEntity));
            }

            return OperationResult<List<Trip>>.Success(result);
        }

        public Task<OperationResult> UpdateTripAsync(Trip trip)
        {
            throw new NotImplementedException();
        }
    }
}
