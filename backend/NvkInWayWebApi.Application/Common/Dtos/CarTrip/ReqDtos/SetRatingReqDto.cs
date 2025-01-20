namespace NvkInWayWebApi.Application.Common.Dtos.CarTrip.ReqDtos
{
    public class SetRatingReqDto
    {
        public Guid TripId { get; set; }
        public long RaterId { get; set; }
        public long TargetId { get; set; }
        public float Rating { get; set; }
    }
}
