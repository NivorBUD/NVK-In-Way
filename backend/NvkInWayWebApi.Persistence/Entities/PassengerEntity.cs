using System;
using System.Collections.Generic;

namespace NvkInWayWebApi.Persistence.Entities;

public partial class PassengerEntity
{
    public Guid Id { get; set; }

    public long TgProfileId { get; set; }

    public int TripCount { get; set; }

    public float? Rating { get; set; }

    public virtual ICollection<RecordEntity> Records { get; set; } = new List<RecordEntity>();
}
