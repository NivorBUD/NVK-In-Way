using System;
using System.Collections.Generic;
using NvkInWayWebApi.Domain.Models;

namespace NvkInWayWebApi.Persistence.Entities;

public partial class CarEntity
{
    public Guid Id { get; set; }

    public long DriverId { get; set; }

    public string Name { get; set; } = null!;

    public string Number { get; set; } = null!;

    public string Color { get; set; } = null!;

    public virtual DriverEntity Driver { get; set; } = null!;

    public bool ImageUploaded { get; set; } = false;

    public virtual ICollection<TripEntity> Trips { get; set; } = new List<TripEntity>();

    public static Car MapFrom(CarEntity carEntity)
    {
        return new Car
        {
            Id = carEntity.Id,
            Name = carEntity.Name,
            Number = carEntity.Number,
            Color = carEntity.Color,
            DriverId = carEntity.DriverId
        };
    }

    public static CarEntity MapFrom(Car car)
    {
        return new CarEntity()
        {
            Id = car.Id,
            Name = car.Name,
            Number = car.Number,
            Color = car.Color,
            DriverId = car.DriverId
        };
    }
}
