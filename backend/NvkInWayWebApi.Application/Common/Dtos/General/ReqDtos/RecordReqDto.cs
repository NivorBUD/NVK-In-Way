namespace NvkInWayWebApi.Application.Common.Dtos.General.ReqDtos
{
    public class RecordReqDto
    {
        public Guid TripId { get; set; }

        public long DriverId { get; set; }

        public long PassengerId { get; set; }
    }
}
