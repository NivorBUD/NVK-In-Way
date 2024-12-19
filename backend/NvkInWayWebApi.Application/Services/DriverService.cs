using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NvkInWayWebApi.Application.Common.Dtos.Driver.ReqDtos;
using NvkInWayWebApi.Application.Common.Dtos.Driver.ResDtos;
using NvkInWayWebApi.Application.Interfaces;
using NvkInWayWebApi.Domain;
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

        public async Task<OperationResult<DriverProfileResDto>> GetDriverProfileByIdAsync(long profileId)
        {
            var driverProfileResult = await repository.GetDriverProfileByIdAsync(profileId);

            if (!driverProfileResult.IsSuccess)
                return OperationResult<DriverProfileResDto>.Error(driverProfileResult.ErrorText);

            var resDto = DriverProfileResDto.MapFrom(driverProfileResult.Data);

            return OperationResult<DriverProfileResDto>.Success(resDto);
        }

        public async Task<OperationResult> DeleteDriverProfileByIdAsync(long profileId)
        {
            return await repository.DeleteDriverAsync(profileId);
        }

        public async Task<OperationResult> AddDriverProfileAsync(DriverProfileReqDto driverProfileReqDto)
        {
            var profileExist = await GetDriverProfileByIdAsync(driverProfileReqDto.TgProfileId);

            if (profileExist.IsSuccess)
                return OperationResult.Error("Профиль уже существует");

            var newProfile = new DriverProfile()
            {
                TgProfileId = driverProfileReqDto.TgProfileId,
                Cars = driverProfileReqDto.Cars.Select(c => new Car()
                {
                    Name = c.AutoName,
                    Number = c.AutoNumber,
                    Color = c.AutoColor
                }).ToList()
            };

            await repository.AddDriverAsync(newProfile);
            return OperationResult.Success();
        }

        public async Task<OperationResult> DeleteDriverCars(long profileId, List<Guid> carIds)
        {
            return await repository.DeleteDriverCarsAsync(profileId, carIds);
        }

        public async Task<OperationResult> AddDriverCars(long profileId, List<CarReqDto> listCarReqDtos)
        {
            var cars = listCarReqDtos
                .Select(c => CarReqDto.MapFrom(c))
                .ToList();

            return await repository.AddDriverCarsAsync(profileId, cars);
        }

        public async Task<OperationResult> UpdateDriverCars(long profileId, List<DetailedСarReqDto> listDetailedCars)
        {
            var cars = listDetailedCars
                .Select(c => DetailedСarReqDto.MapFrom(c))
                .ToList();

            return await repository.UpdateDriverCarsAsync(profileId, cars);
        }
    }
}
