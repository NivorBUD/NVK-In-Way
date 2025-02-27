﻿using NvkInWayWebApi.Application.Common.Dtos.General.ResDtos;
using NvkInWayWebApi.Domain.Models.Profiles;

namespace NvkInWayWebApi.Application.Common.Dtos.Driver.ResDtos
{
    public class DriverProfileResDto
    {
        public List<CarResDto> Cars { get; set; }

        public long TgProfileId { get; set; }

        public double? Rating { get; set; }

        public int AllTripsCount { get; set; }

        public static DriverProfileResDto MapFrom(DriverProfile profile)
        {
            return new()
            {
                TgProfileId = profile.TgProfileId,
                Rating = profile.Rating,
                AllTripsCount = profile.TripsCount,
                Cars = profile.Cars
                    .Select(c => CarResDto.MapFrom(c))
                    .ToList()
            };
        }
    }
}
