using System;
using System.Collections.Generic;

namespace NvkInWayWebApi.Persistence.Entities;

public partial class Location
{
    public string Id { get; set; } = null!;

    public string Description { get; set; } = null!;

    public List<float> Coordinate { get; set; } = null!;

    public virtual ICollection<Taxis> TaxisEndPointNavigations { get; set; } = new List<Taxis>();

    public virtual ICollection<Taxis> TaxisStartPointNavigations { get; set; } = new List<Taxis>();

    public virtual ICollection<Trip> TripEndPointNavigations { get; set; } = new List<Trip>();

    public virtual ICollection<Trip> TripStartPointNavigations { get; set; } = new List<Trip>();
}
