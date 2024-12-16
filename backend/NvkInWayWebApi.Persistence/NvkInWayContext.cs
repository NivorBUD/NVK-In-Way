using Microsoft.EntityFrameworkCore;

namespace NvkInWayWebApi;

public partial class NvkInWayContext : DbContext
{
    public NvkInWayContext()
    {
    }

    public NvkInWayContext(DbContextOptions<NvkInWayContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<Driver> Drivers { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Passenger> Passengers { get; set; }

    public virtual DbSet<Record> Records { get; set; }

    public virtual DbSet<Trip> Trips { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=nvk_in_way;Username=nvk_in_way;Password=nvkthebest");

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
