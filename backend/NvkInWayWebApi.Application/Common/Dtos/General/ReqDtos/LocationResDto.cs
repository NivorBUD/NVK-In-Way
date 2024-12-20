using NvkInWayWebApi.Domain.Models;

namespace NvkInWayWebApi.Application.Common.Dtos.General.ReqDtos
{
    public class LocationResDto
    {
        public string TextDescription { get; set; }

        public Coordinate Coordinate { get; set; }

        public static LocationResDto MapFrom(Location location)
        {
            return new LocationResDto()
            {
                TextDescription = location.TextDescription,
                Coordinate = location.Coordinate,
            };
        }
    }
}
