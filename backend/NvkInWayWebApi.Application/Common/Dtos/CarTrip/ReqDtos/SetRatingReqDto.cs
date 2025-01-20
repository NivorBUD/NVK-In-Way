namespace NvkInWayWebApi.Application.Common.Dtos.CarTrip.ReqDtos
{
    public class SetRatingReqDto
    {
        public Guid TripId { get; set; }
        public long UserId { get; set; }
        public double Rating { get; set; }
    }
}
