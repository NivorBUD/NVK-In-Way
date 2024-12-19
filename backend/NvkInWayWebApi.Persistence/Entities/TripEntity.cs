using System;
using System.Collections.Generic;

namespace NvkInWayWebApi.Persistence.Entities;

public partial class TripEntity
{
    public Guid Id { get; set; }

    public Guid DriverId { get; set; }

    public Guid CarId { get; set; }

    public Guid StartPoint { get; set; }

    public Guid EndPoint { get; set; }

    public DateOnly? DriveStartTime { get; set; }

    public DateOnly? DriveEndTime { get; set; }

    public int TotalPlaces { get; set; }

    public int BookedPlaces { get; set; }

    public virtual CarEntity Car { get; set; } = null!;

    public virtual DriverEntity Driver { get; set; } = null!;

    public virtual LocationEntity EndPointNavigation { get; set; } = null!;

    public virtual ICollection<RecordEntity> Records { get; set; } = new List<RecordEntity>();

    public virtual LocationEntity StartPointNavigation { get; set; } = null!;
}
