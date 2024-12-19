using System;
using System.Collections.Generic;

namespace NvkInWayWebApi.Persistence.Entities;

public partial class TaxisEntity
{
    public string Id { get; set; } = null!;

    public string StartPoint { get; set; } = null!;

    public string EndPoint { get; set; } = null!;

    public DateOnly? DriveStartTime { get; set; }

    public DateOnly? DriveEndTime { get; set; }

    public int CountPlaces { get; set; }

    public virtual LocationEntity EndPointNavigation { get; set; } = null!;

    public virtual LocationEntity StartPointNavigation { get; set; } = null!;
}
