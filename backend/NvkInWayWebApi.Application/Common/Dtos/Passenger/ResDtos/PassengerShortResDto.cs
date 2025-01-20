using NvkInWayWebApi.Domain.Models.Profiles;

namespace NvkInWayWebApi.Application.Common.Dtos.Passenger.ResDtos
{
    public class PassengerShortResDto
    {
        public double? Rating { get; set; }

        public int TripsCount { get; set; }

        public long TgProfileId { get; set; }

        public static PassengerShortResDto MapFrom(PassengerProfile profile)
        {
            return new()
            {
                TgProfileId = profile.TgProfileId,
                Rating = profile.Rating,
                TripsCount = profile.TripsCount
            };
        }
    }
}
