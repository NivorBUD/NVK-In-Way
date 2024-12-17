using System;
using System.Collections.Generic;

namespace NvkInWayWebApi.Persistence.Entities;

public partial class Trip
{
    public string Id { get; set; } = null!;

    public string DriverId { get; set; } = null!;

    public string CarId { get; set; } = null!;

    public string StartPoint { get; set; } = null!;

    public string EndPoint { get; set; } = null!;

    public DateOnly? DriveStartTime { get; set; }

    public DateOnly? DriveEndTime { get; set; }

    public int TotalPlaces { get; set; }

    public int BookedPlaces { get; set; }

    public virtual Car Car { get; set; } = null!;

    public virtual Driver Driver { get; set; } = null!;

    public virtual Location EndPointNavigation { get; set; } = null!;

    public virtual ICollection<Record> Records { get; set; } = new List<Record>();

    public virtual Location StartPointNavigation { get; set; } = null!;
}
