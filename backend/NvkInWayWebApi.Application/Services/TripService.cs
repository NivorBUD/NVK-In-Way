using NvkInWayWebApi.Application.Common.Dtos.CarTrip.ReqDtos;
using NvkInWayWebApi.Application.Common.Dtos.CarTrip.ResDtos;
using NvkInWayWebApi.Application.Common.Dtos.General.ReqDtos;
using NvkInWayWebApi.Application.Interfaces;
using NvkInWayWebApi.Domain;
using NvkInWayWebApi.Domain.Models;
using NvkInWayWebApi.Domain.RepositoriesContract;

namespace NvkInWayWebApi.Application.Services
{
    public class TripService : ITripService
    {
        private readonly ITripRepository repository;

        public TripService(ITripRepository tripRepository)
        {
            repository = tripRepository;
        }

        public async Task<OperationResult> AddDriverTripAsync(CreateTripReqDto tripReqDto)
        {
            // Добавить преобразование из dto в модель(cделано)
            var newTrip = new Trip()
            {
                StartPoint = LocationReqDto.MapFrom(tripReqDto.StartPoint),
                EndPoint = LocationReqDto.MapFrom(tripReqDto.EndPoint),
                StartTime = tripReqDto.DriveStartTime,
                EndTime = tripReqDto.DriveEndTime,
                TotalPlaces = tripReqDto.TotalPlaces,
                Cost = tripReqDto.TripCost,
                CarLocation = tripReqDto.CarLocation,
                DriverId = tripReqDto.TripCar.DriverId,
                DriverCar = OnlyCarIdsReqDto.MapFrom(tripReqDto.TripCar),
            };

            await repository.AddTripAsync(newTrip);
            return OperationResult.Success();
        }

        public async Task<OperationResult> DeleteTripAsync(Guid trip)
        {
            await repository.DeleteTripAsync(trip);
            return OperationResult.Success();
        }

        
        public async Task<OperationResult<List<ShortActiveTripResDto>>> GetShortTripInfoByIntervalAsync(
            IntervalSearchReqDto searchDto, int startIndex, int count)
        {
            var domainSearchInterval = IntervalSearchReqDto.MapFrom(searchDto);
            
            var repositoryResult = await repository.GetTripsByIntervalAsync(domainSearchInterval, startIndex, count);

            if(!repositoryResult.IsSuccess)
                return OperationResult<List<ShortActiveTripResDto>>.Error("Ошибка поиска поездок по интервалу");

            var shortInfo = repositoryResult.Data
                .Select(d => ShortActiveTripResDto.MapFrom(d))
                .ToList();

            return OperationResult<List<ShortActiveTripResDto>>.Success(shortInfo);
        }

        public async Task<OperationResult<List<GetActiveTripsResDto>>> GetTripsByDriverIdAsync(long driverId)
        {
            var tripResult = await repository.GetTripsByDriverIdAsync(driverId);

            if (!tripResult.IsSuccess)
                return OperationResult<List<GetActiveTripsResDto>>.Error(tripResult.ErrorText);

            var resDto = tripResult.Data
                .Select(t => GetActiveTripsResDto.MapFrom(t))
                .ToList();

            return OperationResult<List<GetActiveTripsResDto>>.Success(resDto);
        }

        public async Task<OperationResult> UpdateDriverTripAsync(CreateTripReqDto tripReqDto)
        {
            var updatedTrip = new Trip()
            {
                StartPoint = LocationReqDto.MapFrom(tripReqDto.StartPoint),
                EndPoint = LocationReqDto.MapFrom(tripReqDto.EndPoint),
                StartTime = tripReqDto.DriveStartTime,
                EndTime = tripReqDto.DriveEndTime,
                TotalPlaces = tripReqDto.TotalPlaces,
                Cost = tripReqDto.TripCost,
                CarLocation = tripReqDto.CarLocation,
                DriverCar = OnlyCarIdsReqDto.MapFrom(tripReqDto.TripCar),
            };

            await repository.UpdateTripAsync(updatedTrip);
            return OperationResult.Success();
        }

        public async Task<OperationResult<GetActiveTripsResDto>> GetTripById(Guid id)
        {
            var tripResult = await repository.GetTripByTripIdAsync(id);

            if (!tripResult.IsSuccess)
                return OperationResult<GetActiveTripsResDto>.Error(tripResult.ErrorText);

            var resDto = GetActiveTripsResDto.MapFrom(tripResult.Data);

            return OperationResult<GetActiveTripsResDto>.Success(resDto);
        }
    }
}
