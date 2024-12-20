using System;
using System.Collections.Generic;

namespace NvkInWayWebApi.Persistence.Entities;

public partial class TaxisEntity
{
    public Guid Id { get; set; }

    public Guid StartPoint { get; set; }

    public Guid EndPoint { get; set; }

    public DateOnly? DriveStartTime { get; set; }

    public DateOnly? DriveEndTime { get; set; }

    public int CountPlaces { get; set; }

    public virtual LocationEntity EndPointNavigation { get; set; } = null!;

    public virtual LocationEntity StartPointNavigation { get; set; } = null!;
}
