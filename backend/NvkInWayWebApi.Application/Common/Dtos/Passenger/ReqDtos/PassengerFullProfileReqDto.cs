namespace NvkInWayWebApi.Application.Common.Dtos.Passenger.ReqDtos
{
    public class PassengerFullProfileReqDto
    {
        public float? Rating { get; set; }

        public int TripsCount { get; set; }

        public long TgProfileId { get; set; }
    }
}
