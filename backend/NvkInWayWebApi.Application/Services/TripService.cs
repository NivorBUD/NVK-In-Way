using NvkInWayWebApi.Application.Common.Dtos.Driver.ReqDtos;
using NvkInWayWebApi.Application.Common.Dtos.Driver.ResDtos;
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
            // Добавить преобразование из dto в модель
            var newTrip = new Trip()
            {
                //StartPoint = tripReqDto.StartPoint,
                //EndPoint = tripReqDto.EndPoint,
                StartTime = tripReqDto.DriveStartTime,
                EndTime = tripReqDto.DriveEndTime,
                TotalPlaces = tripReqDto.TotalPlaces,
                Cost = tripReqDto.TripCost,
                //CarLocation = tripReqDto.CarLocation,
                //DriverCar = tripReqDto.TripCar
            };

            await repository.AddTripAsync(newTrip);
            return OperationResult.Success();
        }

        public async Task<OperationResult> DeleteTripAsync(Guid trip)
        {
            await repository.DeleteTripAsync(trip);
            return OperationResult.Success();
        }

        // Добавить обработку списка трипов
        public async Task<OperationResult<GetActiveTripsResDto>> GetTripsByDriverIdAsync(long driverId)
        {
            var tripResult = await repository.GetTripsByDriverIdAsync(driverId);

            if (!tripResult.IsSuccess)
                return OperationResult<GetActiveTripsResDto>.Error(tripResult.ErrorText);

            // Добавить преобразование
            //var resDto = GetActiveTripsResDto.MapFrom(tripResult.Data);

            return OperationResult<GetActiveTripsResDto>.Success();
        }

        public async Task<OperationResult> UpdateDriverTripAsync(CreateTripReqDto tripReqDto)
        {
            // Добавить преобразование из dto в модель, если еще нет
            var updatedTrip = new Trip()
            {
                //StartPoint = tripReqDto.StartPoint,
                //EndPoint = tripReqDto.EndPoint,
                StartTime = tripReqDto.DriveStartTime,
                EndTime = tripReqDto.DriveEndTime,
                TotalPlaces = tripReqDto.TotalPlaces,
                Cost = tripReqDto.TripCost,
                //CarLocation = tripReqDto.CarLocation,
                //DriverCar = tripReqDto.TripCar
            };

            await repository.UpdateTripAsync(updatedTrip);
            return OperationResult.Success();
        }
    }
}
