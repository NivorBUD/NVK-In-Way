using NvkInWayWebApi.Application.Common.Dtos.General.ReqDtos;
using NvkInWayWebApi.Application.Common.Dtos.General.ResDtos;

namespace NvkInWayWebApi.Application.Common.Dtos.CarTrip.ReqDtos
{
    public class CreateTripReqDto
    {
        public LocationReqDto StartPoint { get; set; }

        public LocationReqDto EndPoint { get; set; }

        public DateTime DriveStartTime { get; set; }

        public DateTime DriveEndTime { get; set; }

        public int TotalPlaces { get; set; }

        public double TripCost { get; set; }

        public string CarLocation { get; set; }

        public OnlyCarIdsReqDto TripCar { get; set; }
    }
}
