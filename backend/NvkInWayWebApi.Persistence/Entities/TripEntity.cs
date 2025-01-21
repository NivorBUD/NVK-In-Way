using NvkInWayWebApi.Domain.Models;

namespace NvkInWayWebApi.Persistence.Entities;

public partial class TripEntity
{
    public Guid Id { get; set; }

    public long DriverId { get; set; }

    public Guid CarId { get; set; }

    //location ids start

    public Guid StartPointId { get; set; }

    public Guid EndPointId { get; set; }

    //end location ids

    public string CarLocation { get; set; }

    public DateTimeOffset DriveStartTime { get; set; }

    public DateTimeOffset DriveEndTime { get; set; }

    public int TotalPlaces { get; set; }

    public int BookedPlaces { get; set; }

    public double Costs { get; set; }

    public bool NotifyingProcessed { get; set; }

    //------------------------------------------------

    public virtual CarEntity Car { get; set; } = null!;

    public virtual DriverEntity Driver { get; set; } = null!;

    public virtual LocationEntity EndPointNavigation { get; set; } = null!;

    public virtual LocationEntity StartPointNavigation { get; set; } = null!;

    public virtual ICollection<RecordEntity> Records { get; set; } = new List<RecordEntity>();

    public static TripEntity MapFrom(Trip trip)
    {
        return new TripEntity()
        {
            Id = trip.Id,
            DriverId = trip.DriverId,
            CarId = trip.DriverCar.Id,
            StartPointNavigation = LocationEntity.MapFrom(trip.StartPoint),
            EndPointNavigation = LocationEntity.MapFrom(trip.EndPoint),
            CarLocation = trip.CarLocation,
            DriveStartTime = trip.StartTime.UtcDateTime,
            DriveEndTime = trip.EndTime.UtcDateTime,
            TotalPlaces = trip.TotalPlaces,
            Costs = trip.Cost,
            BookedPlaces = trip.BookedPlaces
        };
    }

    public static Trip MapFrom(TripEntity trip)
    {
        // Добавить машину
        return new Trip()
        {
            Id = trip.Id,
            DriverId = trip.DriverId,
            DriverCar = CarEntity.MapFrom(trip.Car),
            StartPoint = LocationEntity.MapFrom(trip.StartPointNavigation),
            EndPoint = LocationEntity.MapFrom(trip.EndPointNavigation),
            CarLocation = trip.CarLocation,
            StartTime = trip.DriveStartTime.LocalDateTime,
            EndTime = trip.DriveEndTime.LocalDateTime,
            TotalPlaces = trip.TotalPlaces,
            BookedPlaces = trip.BookedPlaces,
            Cost = trip.Costs
        };
    }
}
