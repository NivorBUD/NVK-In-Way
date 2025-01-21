namespace NvkInWayWebApi.Domain.Models
{
    public class Trip
    {
        public Guid Id { get; set; }

        public long DriverId { get; set; }

        public Location StartPoint { get; set; }

        public Location EndPoint { get; set; }

        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; }

        public int TotalPlaces { get; set; }

        public int BookedPlaces { get; set; }

        public double Cost { get; set; }

        public string CarLocation { get; set; }

        public bool NotifyingProcessed { get; set; }

        public Car DriverCar { get; set; }
    }
}
