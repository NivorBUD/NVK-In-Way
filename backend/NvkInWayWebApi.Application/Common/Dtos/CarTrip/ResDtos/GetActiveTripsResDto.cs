using NvkInWayWebApi.Application.Common.Dtos.General.ReqDtos;
using NvkInWayWebApi.Application.Common.Dtos.General.ResDtos;
using NvkInWayWebApi.Domain.Models;

namespace NvkInWayWebApi.Application.Common.Dtos.CarTrip.ResDtos
{
    public class GetActiveTripsResDto
    {
        public Guid Id { get; set; }

        public string CarLocation { get; set; }

        public LocationResDto StartPoint { get; set; }

        public LocationResDto EndPoint { get; set; }

        public DateTime TripStartTime { get; set; }

        public DateTime TripEndTime { get; set; }

        public int TotalPlaces { get; set; }

        public double DriveCost { get; set; }

        public int BookedPlaces { get; set; }

        public CarResDto TripCar { get; set; }

        public long DriverId { get; set; }

        public static GetActiveTripsResDto MapFrom(Trip trip)
        {
            return new GetActiveTripsResDto
            {
                Id = trip.Id,
                CarLocation = trip.CarLocation,
                StartPoint = LocationResDto.MapFrom(trip.StartPoint),
                EndPoint = LocationResDto.MapFrom(trip.EndPoint),
                TripStartTime = trip.StartTime,
                TripEndTime = trip.EndTime,
                TotalPlaces = trip.TotalPlaces,
                DriveCost = trip.Cost,
                BookedPlaces = trip.BookedPlaces,
                DriverId = trip.DriverId,
                TripCar = CarResDto.MapFrom(trip.DriverCar),
            };
        }
    }
}
