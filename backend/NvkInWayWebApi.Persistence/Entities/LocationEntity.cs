using System;
using System.Collections.Generic;

namespace NvkInWayWebApi.Persistence.Entities;

public partial class LocationEntity
{
    public string Id { get; set; } = null!;

    public string Description { get; set; } = null!;

    public List<float> Coordinate { get; set; } = null!;

    public virtual ICollection<TaxisEntity> TaxisEndPointNavigations { get; set; } = new List<TaxisEntity>();

    public virtual ICollection<TaxisEntity> TaxisStartPointNavigations { get; set; } = new List<TaxisEntity>();

    public virtual ICollection<TripEntity> TripEndPointNavigations { get; set; } = new List<TripEntity>();

    public virtual ICollection<TripEntity> TripStartPointNavigations { get; set; } = new List<TripEntity>();
}
