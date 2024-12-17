using System;
using System.Collections.Generic;

namespace NvkInWayWebApi.Persistence.Entities;

public partial class Car
{
    public string Id { get; set; } = null!;

    public string DriverId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Number { get; set; } = null!;

    public string Color { get; set; } = null!;

    public virtual Driver Driver { get; set; } = null!;

    public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();
}
