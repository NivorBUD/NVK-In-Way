using NvkInWayWebApi.Domain.Models;

namespace NvkInWayWebApi.Persistence.Entities;

public partial class TripEntity
{
    public Guid Id { get; set; }

    public long DriverId { get; set; }

    public Guid CarId { get; set; }

    public Location StartPoint { get; set; }

    public Location EndPoint { get; set; }

    public DateTime DriveStartTime { get; set; }

    public DateTime DriveEndTime { get; set; }

    public int TotalPlaces { get; set; }

    public int BookedPlaces { get; set; }

    public virtual CarEntity Car { get; set; } = null!;

    public virtual DriverEntity Driver { get; set; } = null!;

    public virtual LocationEntity EndPointNavigation { get; set; } = null!;

    public virtual ICollection<RecordEntity> Records { get; set; } = new List<RecordEntity>();

    public virtual LocationEntity StartPointNavigation { get; set; } = null!;
}
