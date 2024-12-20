using NvkInWayWebApi.Domain.Models;

namespace NvkInWayWebApi.Application.Common.Dtos.General.ReqDtos
{
    public class LocationReqDto
    {
        public string TextDescription { get; set; }

        public Coordinate? Coordinate { get; set; }

        public static Location MapFrom(LocationReqDto resDto)
        {
            if (resDto.Coordinate == null)
            {
                resDto.Coordinate = new Coordinate(null, null);
            }

            return new Location(resDto.TextDescription, resDto.Coordinate);
        }
    }
}
