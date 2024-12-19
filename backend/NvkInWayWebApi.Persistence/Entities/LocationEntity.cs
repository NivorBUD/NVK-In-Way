using System;
using System.Collections.Generic;
using NvkInWayWebApi.Domain.Models;

namespace NvkInWayWebApi.Persistence.Entities;

public partial class LocationEntity
{
    public Guid Id { get; set; }

    public string Description { get; set; } = null!;

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public virtual ICollection<TaxisEntity> TaxisEndPointNavigations { get; set; } = new List<TaxisEntity>();

    public virtual ICollection<TaxisEntity> TaxisStartPointNavigations { get; set; } = new List<TaxisEntity>();

    public virtual ICollection<TripEntity> TripEndPointNavigations { get; set; } = new List<TripEntity>();

    public virtual ICollection<TripEntity> TripStartPointNavigations { get; set; } = new List<TripEntity>();

    public static Location MapFrom(LocationEntity entity)
    {
        var latitude = entity.Latitude;
        var longitude = entity.Longitude;
        return new Location(entity.Description, new Coordinate(latitude, longitude));
    }

    public static LocationEntity MapFrom(Location location)
    {
        return new LocationEntity
        {
            Id = location.Id,
            Latitude = location.Coordinate.Latitude,
            Longitude = location.Coordinate.Longitude,
            Description = location.TextDescription
        };
    }
}
