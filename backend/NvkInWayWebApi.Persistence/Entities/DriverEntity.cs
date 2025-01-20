namespace NvkInWayWebApi.Persistence.Entities;

public partial class DriverEntity
{
    public long TgProfileId { get; set; }

    public float? Rating { get; set; }

    public int TripsCount { get; set; }

    public float? TotalRating { get; set; }

    public int RatingCount { get; set; }


    public virtual ICollection<CarEntity> Cars { get; set; } = new List<CarEntity>();

    public virtual ICollection<RecordEntity> Records { get; set; } = new List<RecordEntity>();

    public virtual ICollection<TripEntity> Trips { get; set; } = new List<TripEntity>();
}
