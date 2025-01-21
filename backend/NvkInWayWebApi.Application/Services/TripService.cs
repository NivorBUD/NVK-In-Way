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
        private readonly ITripRepository tripRepository;

        private readonly IDriverRepository driverRepository;

        public TripService(ITripRepository tripRepository, IDriverRepository driverRepository)
        {
            this.tripRepository = tripRepository;
            this.driverRepository = driverRepository;
        }

        public async Task<OperationResult> AddDriverTripAsync(CreateTripReqDto tripReqDto)
        {
            var driverResult = await driverRepository.GetDriverProfileByIdAsync(tripReqDto.TripCar.DriverId);

            if (!driverResult.IsSuccess)
            {
                return OperationResult.Error(driverResult.ErrorText);
            }

            if (driverResult.Data.Cars.All(car => car.Id != tripReqDto.TripCar.Id))
            {
                return OperationResult.Error("У водителя нет такого автомобиля");
            }

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

            await tripRepository.AddTripAsync(newTrip);
            return OperationResult.Success();
        }

        public async Task<OperationResult> DeleteTripAsync(Guid trip)
        {
            await tripRepository.DeleteTripAsync(trip);
            return OperationResult.Success();
        }


        public async Task<OperationResult<List<ShortActiveTripResDto>>> GetShortTripInfoByIntervalAsync(
            IntervalSearchReqDto searchDto, int startIndex, int count)
        {
            var domainSearchInterval = IntervalSearchReqDto.MapFrom(searchDto);

            var tripRepositoryResult = await tripRepository.GetTripsByIntervalAsync(domainSearchInterval, startIndex, count);

            if (!tripRepositoryResult.IsSuccess)
                return OperationResult<List<ShortActiveTripResDto>>.Error("Ошибка поиска поездок по интервалу");

            var shortInfo = tripRepositoryResult.Data
                .Select(d => ShortActiveTripResDto.MapFrom(d))
                .ToList();

            return OperationResult<List<ShortActiveTripResDto>>.Success(shortInfo);
        }

        public async Task<OperationResult<List<GetActiveTripsResDto>>> GetTripsByDriverIdAsync(long driverId, int startIndex, int count)
        {
            var tripResult = await tripRepository.GetTripsByDriverIdAsync(driverId, startIndex, count);

            if (!tripResult.IsSuccess)
                return OperationResult<List<GetActiveTripsResDto>>.Error(tripResult.ErrorText);

            var resDto = tripResult.Data
                .Select(t => GetActiveTripsResDto.MapFrom(t))
                .ToList();

            return OperationResult<List<GetActiveTripsResDto>>.Success(resDto);
        }

        public async Task<OperationResult<List<GetActiveTripsResDto>>> GetTripsByPassengerIdAsync(
            long passengerId, int startIndex, int count)
        {
            var tripResult = await tripRepository.GetTripsByPassengerIdAsync(passengerId, startIndex, count);

            if (!tripResult.IsSuccess)
                return OperationResult<List<GetActiveTripsResDto>>.Error(tripResult.ErrorText);

            var resDto = tripResult.Data
                .Select(t => GetActiveTripsResDto.MapFrom(t))
                .ToList();

            return OperationResult<List<GetActiveTripsResDto>>.Success(resDto);
        }

        public async Task<OperationResult<List<GetActiveTripsResDto>>> GetActiveTripsByDriverIdAsync(
            long driverId, int startIndex, int count)
        {
            var tripResult = await tripRepository.GetActiveTripsByDriverIdAsync(driverId, startIndex, count);

            if (!tripResult.IsSuccess)
                return OperationResult<List<GetActiveTripsResDto>>.Error(tripResult.ErrorText);

            var resDto = tripResult.Data
                .Select(t => GetActiveTripsResDto.MapFrom(t))
                .ToList();

            return OperationResult<List<GetActiveTripsResDto>>.Success(resDto);
        }

        public async Task<OperationResult<List<GetActiveTripsResDto>>> GetActiveTripsByPassengerIdAsync(
            long passengerId, int startIndex, int count)
        {
            var tripResult = await tripRepository.GetActiveTripsByPassengerIdAsync(passengerId, startIndex, count);

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

            await tripRepository.UpdateTripAsync(updatedTrip);
            return OperationResult.Success();
        }

        public async Task<OperationResult<GetActiveTripsResDto>> GetTripById(Guid id)
        {
            var tripResult = await tripRepository.GetTripByTripIdAsync(id);

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

            return await tripRepository.RecordToTripAsync(record);
        }

        public async Task<OperationResult> CompleteTripAsync(EndTripReqDto endTripReqDto)
        {
            var endTrip = new Trip
            {
                Id = endTripReqDto.TripId,
                DriverId = endTripReqDto.DriverId
            };

            return await tripRepository.CompleteTripAsync(endTrip);
        }

        public async Task<OperationResult> RateParticipantAsync(SetRatingReqDto setRatingReqDto)
        {
            return await tripRepository.RateParticipantAsync(setRatingReqDto.TripId, setRatingReqDto.RaterId, setRatingReqDto.TargetId, setRatingReqDto.Rating);
        }

        public async Task<OperationResult<List<NotifyTripResDto>>> GetNotifyingProfilesFromTrips(int startTripIndex,
            int tripCount)
        {
            var result = await tripRepository.GetNotifyingProfilesFromTrips(startTripIndex, tripCount);

            if (!result.IsSuccess)
                return OperationResult<List<NotifyTripResDto>>.Error(result.ErrorText);

            return OperationResult<List<NotifyTripResDto>>.Success(result.Data
                .Select(x => NotifyTripResDto.MapFrom(x))
                .ToList());
        }
    }
}
