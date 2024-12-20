using NvkInWayWebApi.Application.Common.Dtos.General.ReqDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NvkInWayWebApi.Domain.Models;

namespace NvkInWayWebApi.Application.Common.Dtos.CarTrip.ResDtos
{
    public class ShortActiveTripResDto
    {
        public Guid Id { get; set; }

        public LocationResDto StartPoint { get; set; }

        public LocationResDto EndPoint { get; set; }

        public DateTime TripStartTime { get; set; }

        public DateTime TripEndTime { get; set; }

        public int FreePlaces { get; set; }

        public static ShortActiveTripResDto MapFrom(Trip trip)
        {
            return new ShortActiveTripResDto()
            {
                Id = trip.Id,
                StartPoint = LocationResDto.MapFrom(trip.StartPoint),
                EndPoint = LocationResDto.MapFrom(trip.EndPoint),
                TripStartTime = trip.StartTime,
                TripEndTime = trip.EndTime,
                FreePlaces = trip.TotalPlaces - trip.BookedPlaces
            };
        }
    }
}
