using NvkInWayWebApi.Application.Common.Dtos.General.ReqDtos;
using NvkInWayWebApi.Application.Common.Dtos.General.ResDtos;
using NvkInWayWebApi.Domain.Models;

namespace NvkInWayWebApi.Application.Common.Dtos.CarTrip
{
    public class GetActiveTripsResDto
    {
        public string CarLocation { get; set; }

        public LocationResDto StartPoint { get; set; }

        public LocationResDto EndPoint { get; set; }

        public DateTime TripStartTime { get; set; }

        public DateTime TripEndTime { get; set; }

        public int TotalPlaces { get; set; }

        public double DriveCost { get; set; }

        public int BookedPlaces { get; set; }

        public CarResDto TripCar { get; set; }

        public static GetActiveTripsResDto MapFrom(Trip trip)
        {
            return new GetActiveTripsResDto
            {
                CarLocation = trip.CarLocation,
                StartPoint = LocationResDto.MapFrom(trip.StartPoint),
                EndPoint = LocationResDto.MapFrom(trip.EndPoint),
                TripStartTime = trip.StartTime,
                TripEndTime = trip.EndTime,
                TotalPlaces = trip.TotalPlaces,
                DriveCost = trip.Cost,
                BookedPlaces = trip.BookedPlaces,
                TripCar = CarResDto.MapFrom(trip.DriverCar),
            };
        }
    }
}
