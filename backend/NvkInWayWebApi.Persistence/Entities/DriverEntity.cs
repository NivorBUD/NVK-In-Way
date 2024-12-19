using System;
using System.Collections.Generic;

namespace NvkInWayWebApi.Persistence.Entities;

public partial class DriverEntity
{
    public Guid Id { get; set; }

    public long TgProfileId { get; set; }

    public float? Rating { get; set; }

    public int AllTripsCount { get; set; }

    public virtual ICollection<CarEntity> Cars { get; set; } = new List<CarEntity>();

    public virtual ICollection<RecordEntity> Records { get; set; } = new List<RecordEntity>();

    public virtual ICollection<TripEntity> Trips { get; set; } = new List<TripEntity>();
}
