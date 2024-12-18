using System;
using System.Collections.Generic;

namespace NvkInWayWebApi.Persistence.Entities;

public partial class Driver
{
    public string Id { get; set; } = null!;

    public long TgProfileId { get; set; }

    public float? Rating { get; set; }

    public int AllTripsCount { get; set; }

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();

    public virtual ICollection<Record> Records { get; set; } = new List<Record>();

    public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();
}
