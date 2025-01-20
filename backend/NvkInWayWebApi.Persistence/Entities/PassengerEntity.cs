namespace NvkInWayWebApi.Persistence.Entities;

public partial class PassengerEntity
{
    public long TgProfileId { get; set; }

    public int TripsCount { get; set; }

    public double? Rating { get; set; }

    public double TotalRating { get; set; }

    public int RatingCount { get; set; }

    public virtual ICollection<RecordEntity> Records { get; set; } = new List<RecordEntity>();
}
