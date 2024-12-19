using System;
using System.Collections.Generic;

namespace NvkInWayWebApi.Persistence.Entities;

public partial class RecordEntity
{
    public string Id { get; set; } = null!;

    public string DriverId { get; set; } = null!;

    public string TripId { get; set; } = null!;

    public string PassengerId { get; set; } = null!;

    public virtual DriverEntity Driver { get; set; } = null!;

    public virtual PassengerEntity Passenger { get; set; } = null!;

    public virtual TripEntity Trip { get; set; } = null!;
}
