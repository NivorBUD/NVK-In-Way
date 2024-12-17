using System;
using System.Collections.Generic;

namespace NvkInWayWebApi.Persistence.Entities;

public partial class Record
{
    public string Id { get; set; } = null!;

    public string DriverId { get; set; } = null!;

    public string TripId { get; set; } = null!;

    public string PassengerId { get; set; } = null!;

    public virtual Driver Driver { get; set; } = null!;

    public virtual Passenger Passenger { get; set; } = null!;

    public virtual Trip Trip { get; set; } = null!;
}
