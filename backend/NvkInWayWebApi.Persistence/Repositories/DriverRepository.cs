using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NvkInWayWebApi.Domain;
using NvkInWayWebApi.Domain.Models;
using NvkInWayWebApi.Domain.Models.Profiles;
using NvkInWayWebApi.Domain.RepositoriesContract;
using NvkInWayWebApi.Persistence.Entities;

namespace NvkInWayWebApi.Persistence.Repositories
{
    public class DriverRepository : CommonRepository<DriverEntity>, IDriverRepository
    {
        public DriverRepository(DbContext context) : base(context) { }

        public async Task<OperationResult> AddDriverAsync(DriverProfile driver)
        {
            await AddAsync(MapFrom(driver));
            await SaveChangesAsync();
            return OperationResult.Success(201);
        }

        public async Task<OperationResult> DeleteDriverAsync(long profileId)
        {
            var driver = await _dbSet.FirstOrDefaultAsync(d => d.TgProfileId == profileId);
            if (driver != null)
            {
                Delete(driver);
                await SaveChangesAsync();
                return OperationResult.Success(204);
            }
            return OperationResult.Error("Профиль для удаления не обнаружен");
        }

        public async Task<OperationResult<DriverProfile>> GetDriverProfileByIdAsync(long profileId)
        {
            var dbEntity = await _context.Set<DriverEntity>()
                .Include(d => d.Cars)
                .FirstOrDefaultAsync(d => d.TgProfileId == profileId);

            if (dbEntity == null)
                return OperationResult<DriverProfile>.Error("Пользователь с таким профилем не найден");

            var profile = MapFrom(dbEntity);

            return OperationResult<DriverProfile>.Success(profile);
        }

        public async Task<OperationResult> UpdateDriverCarsAsync(long driverId, List<Car> cars)
        {
            if (cars == null || !cars.Any())
                return OperationResult.Error("Список машин не может быть пустым");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var dbEntity = await _context.Set<DriverEntity>()
                    .Include(d => d.Cars)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(d => d.TgProfileId == driverId);

                if (dbEntity == null)
                    return OperationResult.Error("Водитель не найден");

                foreach (var car in cars)
                {
                    if (!dbEntity.Cars.Any(c => c.Id == car.Id))
                        return OperationResult.Error(
                            $"Обновление не возможно у пользователя не существует машина с таким id = {car.Id}");
                }

                foreach (var car in cars)
                {
                    _context.Set<CarEntity>().Update(new CarEntity()
                    {
                        Id = car.Id,
                        DriverId = dbEntity.TgProfileId,
                        Color = car.Color,
                        Name = car.Name,
                        Number = car.Number
                    });
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync(); // Завершение транзакции
            }
            catch (DbUpdateConcurrencyException ex)
            {
                await transaction.RollbackAsync(); // Откат транзакции
                return OperationResult.Error("Произошла ошибка при добавлении автомобилей. Возможно, данные были изменены.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); // Откат транзакции
                return OperationResult.Error("Произошла ошибка: " + ex.Message);
            }

            return OperationResult.Success(204);
        }

        public async Task<OperationResult> AddDriverCarsAsync(long driverId, List<Car> cars)
        {
            if (cars == null || !cars.Any())
                return OperationResult.Error("Список машин не может быть пустым");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var dbEntity = await _context.Set<DriverEntity>()
                    .Include(d => d.Cars)
                    .FirstOrDefaultAsync(d => d.TgProfileId == driverId);

                if (dbEntity == null)
                    return OperationResult.Error("Водитель не найден");

                foreach (var car in cars)
                {
                    if (dbEntity.Cars.Any(c => c.Id == car.Id))
                        return OperationResult.Error($"У пользователя уже существует машина с таким id = {car.Id}");
                }

                foreach (var car in cars)
                {
                    await _context.Set<CarEntity>().AddAsync(new CarEntity()
                    {
                        Id = Guid.NewGuid(), // Генерация нового идентификатора
                        DriverId = dbEntity.TgProfileId,
                        Color = car.Color,
                        Name = car.Name,
                        Number = car.Number
                    });
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync(); // Завершение транзакции
            }
            catch (DbUpdateConcurrencyException ex)
            {
                await transaction.RollbackAsync(); // Откат транзакции
                return OperationResult.Error("Произошла ошибка при добавлении автомобилей. Возможно, данные были изменены.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); // Откат транзакции
                return OperationResult.Error("Произошла ошибка: " + ex.Message);
            }

            return OperationResult.Success(204);
        }



        public async Task<OperationResult> DeleteDriverCarsAsync(long driverId, List<Guid> carsIds)
        {
            var dbEntity = await _context.Set<DriverEntity>()
                .Include(d => d.Cars)
                .FirstOrDefaultAsync(d => d.TgProfileId == driverId);

            if (dbEntity == null)
                return OperationResult.Error("Водитель не найден");

            foreach (var carId in carsIds)
            {
                if (!dbEntity.Cars.Any(c => c.Id == carId))
                    return OperationResult.Error("У пользователя не существует какой-либо из переданных машин");
            }

            foreach (var carId in carsIds)
            {
                var carToRemove = dbEntity.Cars.FirstOrDefault(c => c.Id == carId);
                dbEntity.Cars.Remove(carToRemove);
            }

            await _context.SaveChangesAsync();

            return OperationResult.Success(204);
        }

        public DriverEntity MapFrom(DriverProfile profile)
        {
            return new DriverEntity
            {
                TgProfileId = profile.TgProfileId,
                Rating = profile.Rating,
                AllTripsCount = profile.TripsCount,
                Cars = profile.Cars
                    .Select(c => MapFrom(c))
                    .ToList(),
            };
        }

        public DriverProfile MapFrom(DriverEntity driverEntity)
        {
            return new DriverProfile
            {
                TgProfileId = driverEntity.TgProfileId,
                Rating = driverEntity.Rating,
                TripsCount = driverEntity.AllTripsCount,
                Cars = driverEntity.Cars
                    .Select(c => MapFrom(c))
                    .ToList(),
            };
        }

        public Car MapFrom(CarEntity carEntity)
        {
            return new Car
            {
                Id = carEntity.Id,
                DriverId = carEntity.DriverId,
                Name = carEntity.Name,
                Number = carEntity.Number,
                Color = carEntity.Color
            };
        }

        public CarEntity MapFrom(Car carEntity)
        {
            return new CarEntity
            {
                Id = carEntity.Id,
                DriverId = carEntity.DriverId,
                Name = carEntity.Name,
                Number = carEntity.Number,
                Color = carEntity.Color
            };
        }
    }
}
