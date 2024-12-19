using System;
using System.Collections.Generic;

namespace NvkInWayWebApi.Persistence.Entities;

public partial class CarEntity
{
    public Guid Id { get; set; }

    public Guid DriverId { get; set; }

    public string Name { get; set; } = null!;

    public string Number { get; set; } = null!;

    public string Color { get; set; } = null!;

    public virtual DriverEntity Driver { get; set; } = null!;

    public virtual ICollection<TripEntity> Trips { get; set; } = new List<TripEntity>();
}
