using System;
using System.Collections.Generic;

namespace NvkInWayWebApi.Persistence.Entities;

public partial class RecordEntity
{
    public Guid Id { get; set; }

    public long DriverId { get; set; }

    public Guid TripId { get; set; }

    public long PassengerId { get; set; }

    public virtual DriverEntity Driver { get; set; } = null!;

    public virtual PassengerEntity Passenger { get; set; } = null!;

    public virtual TripEntity Trip { get; set; } = null!;
}
