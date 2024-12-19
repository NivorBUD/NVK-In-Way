using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NvkInWayWebApi.Domain.Models;
using NvkInWayWebApi.Domain.Models.Profiles;
using NvkInWayWebApi.Domain.RepositoriesContract;
using NvkInWayWebApi.Persistence.Entities;

namespace NvkInWayWebApi.Persistence.Repositories
{
    public class DriverRepository : CommonRepository<DriverEntity>, IDriverRepository
    {
        public DriverRepository(DbContext context) : base(context) { }

        public async Task AddDriverAsync(DriverProfile driver)
        {
            await AddAsync(MapFrom(driver));
            await SaveChangesAsync();
        }

        public async Task UpdateDriverAsync(DriverProfile driver)
        {
            Update(MapFrom(driver));
            await SaveChangesAsync();
        }

        public async Task DeleteDriverAsync(Guid id)
        {
            var driver = await GetByIdAsync(id);
            if (driver != null)
            {
                Delete(driver);
                await SaveChangesAsync();
            }
        }

        public DriverEntity MapFrom(DriverProfile profile)
        {
            return new DriverEntity
            {
                Id = profile.Id,
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
                Id = driverEntity.Id,
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
