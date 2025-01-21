using NvkInWayWebApi.Application.Common.Dtos.CarTrip.ReqDtos;
using NvkInWayWebApi.Application.Common.Dtos.CarTrip.ResDtos;
using NvkInWayWebApi.Application.Common.Dtos.General.ReqDtos;
using NvkInWayWebApi.Application.Interfaces;
using NvkInWayWebApi.Domain;
using NvkInWayWebApi.Domain.Models;
using NvkInWayWebApi.Domain.Models.Profiles;
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

            if (!repositoryResult.IsSuccess)
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

        public async Task<OperationResult<List<GetActiveTripsResDto>>> GetTripsByPassengerIdAsync(long passengerId)
        {
            var tripResult = await repository.GetTripsByPassengerIdAsync(passengerId);

            if (!tripResult.IsSuccess)
                return OperationResult<List<GetActiveTripsResDto>>.Error(tripResult.ErrorText);

            var resDto = tripResult.Data
                .Select(t => GetActiveTripsResDto.MapFrom(t))
                .ToList();

            return OperationResult<List<GetActiveTripsResDto>>.Success(resDto);
        }

        public async Task<OperationResult<List<GetActiveTripsResDto>>> GetActiveTripsByDriverIdAsync(long driverId)
        {
            var tripResult = await repository.GetActiveTripsByDriverIdAsync(driverId);

            if (!tripResult.IsSuccess)
                return OperationResult<List<GetActiveTripsResDto>>.Error(tripResult.ErrorText);

            var resDto = tripResult.Data
                .Select(t => GetActiveTripsResDto.MapFrom(t))
                .ToList();

            return OperationResult<List<GetActiveTripsResDto>>.Success(resDto);
        }

        public async Task<OperationResult<List<GetActiveTripsResDto>>> GetActiveTripsByPassengerIdAsync(long passengerId)
        {
            var tripResult = await repository.GetActiveTripsByPassengerIdAsync(passengerId);

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

        public async Task<OperationResult> RecordToTrip(RecordReqDto recordReqDto)
        {
            var record = new Record
            {
                Id = Guid.NewGuid(),
                Driver = new DriverProfile { TgProfileId = recordReqDto.DriverId },
                Passenger = new PassengerProfile { TgProfileId = recordReqDto.PassengerId },
                Trip = new Trip { Id = recordReqDto.TripId }
            };

            return await repository.RecordToTripAsync(record);
        }

        public async Task<OperationResult> CompleteTripAsync(EndTripReqDto endTripReqDto)
        {
            var endTrip = new Trip
            {
                Id = endTripReqDto.TripId,
                DriverId = endTripReqDto.DriverId
            };

            return await repository.CompleteTripAsync(endTrip);
        }

        public async Task<OperationResult> RateParticipantAsync(SetRatingReqDto setRatingReqDto)
        {
            return await repository.RateParticipantAsync(setRatingReqDto.TripId, setRatingReqDto.RaterId, setRatingReqDto.TargetId, setRatingReqDto.Rating);
        }
    }
}
