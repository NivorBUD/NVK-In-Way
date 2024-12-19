using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NvkInWayWebApi.Application.Common.Dtos.Driver.ReqDtos;
using NvkInWayWebApi.Application.Common.Dtos.Driver.ResDtos;
using NvkInWayWebApi.Application.Interfaces;
using NvkInWayWebApi.Domain.Models;
using NvkInWayWebApi.Domain.Models.Profiles;
using NvkInWayWebApi.Domain.RepositoriesContract;
using NvkInWayWebApi.Persistence.Entities;
using NvkInWayWebApi.Persistence.Repositories;

namespace NvkInWayWebApi.Application.Services
{
    public class DriverService : IDriverService
    {
        private readonly IDriverRepository repository;

        public DriverService(IDriverRepository driverRepository)
        {
            repository = driverRepository;
        }

        public async Task<DriverProfileResDto> GetDriverProfileByIdAsync(long profileId)
        {
            var driverProfile = await repository.GetDriverProfileByIdAsync(profileId);

            if (driverProfile == null)
                return null;

            return DriverProfileResDto.MapFrom(driverProfile);
        }

        public async Task AddDriverProfileAsync(DriverProfileReqDto driverProfileReqDto)
        {
            var newProfile = new DriverProfile()
            {
                TgProfileId = driverProfileReqDto.TgProfileId,
                Rating = driverProfileReqDto.Rating,
                TripsCount = driverProfileReqDto.AllTripsCount,
                Cars = driverProfileReqDto.Cars.Select(c => new Car()
                {
                    Name = c.AutoName,
                    Number = c.AutoNumber,
                    Color = c.AutoColor
                }).ToList()
            };

            await repository.AddDriverAsync(newProfile);
        }
    }
}
