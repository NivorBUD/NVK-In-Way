using NvkInWayWebApi.Application.Common.Dtos.General.ReqDtos;
using NvkInWayWebApi.Application.Common.Dtos.General.ResDtos;
using NvkInWayWebApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NvkInWayWebApi.Application.Common.Dtos.CarTrip.ResDtos
{
    public class NotifyTripInfo
    {
        public LocationResDto StartPoint { get; set; }

        public LocationResDto EndPoint { get; set; }

        public DateTime TripStartTime { get; set; }

        public DateTime TripEndTime { get; set; }

        public CarResDto TripCar { get; set; }

        public string CarLocation { get; set; }

        public static NotifyTripInfo MapFrom(Trip trip)
        {
            return new NotifyTripInfo
            {
                CarLocation = trip.CarLocation,
                StartPoint = LocationResDto.MapFrom(trip.StartPoint),
                EndPoint = LocationResDto.MapFrom(trip.EndPoint),
                TripStartTime = trip.StartTime.LocalDateTime,
                TripEndTime = trip.EndTime.LocalDateTime,
                TripCar = CarResDto.MapFrom(trip.DriverCar),
            };
        }
    }
}
