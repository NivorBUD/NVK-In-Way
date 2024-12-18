using Microsoft.EntityFrameworkCore;
using NvkInWayWebApi.Persistence.Entities;

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

    public virtual DbSet<Taxis> Taxes { get; set; }

    public virtual DbSet<Trip> Trips { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=nvk_in_way;Username=nvk_in_way;Password=nvkthebest");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cars_pkey");

            entity.ToTable("cars");

            entity.Property(e => e.Id)
                .HasMaxLength(40)
                .HasColumnName("id");
            entity.Property(e => e.Color)
                .HasMaxLength(20)
                .HasColumnName("color");
            entity.Property(e => e.DriverId)
                .HasMaxLength(40)
                .HasColumnName("driver_id");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("name");
            entity.Property(e => e.Number)
                .HasMaxLength(8)
                .HasColumnName("number");

            entity.HasOne(d => d.Driver).WithMany(p => p.Cars)
                .HasForeignKey(d => d.DriverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("car_driver_id_fkey");
        });

        modelBuilder.Entity<Driver>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("driver_id_pkey");

            entity.ToTable("drivers");

            entity.Property(e => e.Id)
                .HasMaxLength(40)
                .HasColumnName("id");
            entity.Property(e => e.AllTripsCount).HasColumnName("all_trips_count");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.TgProfileId).HasColumnName("tg_profile_id");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("locations_pkey");

            entity.ToTable("locations");

            entity.Property(e => e.Id)
                .HasMaxLength(40)
                .HasColumnName("id");
            entity.Property(e => e.Coordinate).HasColumnName("coordinate");
            entity.Property(e => e.Description).HasColumnName("description");
        });

        modelBuilder.Entity<Passenger>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("passengers_pkey");

            entity.ToTable("passengers");

            entity.Property(e => e.Id)
                .HasMaxLength(40)
                .HasColumnName("id");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.TgProfileId).HasColumnName("tg_profile_id");
            entity.Property(e => e.TripCount).HasColumnName("trip_count");
        });

        modelBuilder.Entity<Record>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("claims_pkey");

            entity.ToTable("records");

            entity.Property(e => e.Id)
                .HasMaxLength(40)
                .HasColumnName("id");
            entity.Property(e => e.DriverId)
                .HasMaxLength(40)
                .HasColumnName("driver_id");
            entity.Property(e => e.PassengerId)
                .HasMaxLength(40)
                .HasColumnName("passenger_id");
            entity.Property(e => e.TripId)
                .HasMaxLength(40)
                .HasColumnName("trip_id");

            entity.HasOne(d => d.Driver).WithMany(p => p.Records)
                .HasForeignKey(d => d.DriverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("claim_driver_id_fkey");

            entity.HasOne(d => d.Passenger).WithMany(p => p.Records)
                .HasForeignKey(d => d.PassengerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("claim_passenger_id_fkey");

            entity.HasOne(d => d.Trip).WithMany(p => p.Records)
                .HasForeignKey(d => d.TripId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("claim_trip_id_fkey");
        });

        modelBuilder.Entity<Taxis>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("taxis_pkey");

            entity.ToTable("taxis");

            entity.Property(e => e.Id)
                .HasMaxLength(40)
                .HasColumnName("id");
            entity.Property(e => e.CountPlaces).HasColumnName("count_places");
            entity.Property(e => e.DriveEndTime).HasColumnName("drive_end_time");
            entity.Property(e => e.DriveStartTime).HasColumnName("drive_start_time");
            entity.Property(e => e.EndPoint)
                .HasMaxLength(40)
                .HasColumnName("end_point");
            entity.Property(e => e.StartPoint)
                .HasMaxLength(40)
                .HasColumnName("start_point");

            entity.HasOne(d => d.EndPointNavigation).WithMany(p => p.TaxisEndPointNavigations)
                .HasForeignKey(d => d.EndPoint)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("taxis_end_location_id_fkey");

            entity.HasOne(d => d.StartPointNavigation).WithMany(p => p.TaxisStartPointNavigations)
                .HasForeignKey(d => d.StartPoint)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("taxis_start_location_id_fkey");
        });

        modelBuilder.Entity<Trip>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("trips_pkey");

            entity.ToTable("trips");

            entity.Property(e => e.Id)
                .HasMaxLength(40)
                .HasColumnName("id");
            entity.Property(e => e.BookedPlaces).HasColumnName("booked_places");
            entity.Property(e => e.CarId)
                .HasMaxLength(40)
                .HasColumnName("car_id");
            entity.Property(e => e.DriveEndTime).HasColumnName("drive_end_time");
            entity.Property(e => e.DriveStartTime).HasColumnName("drive_start_time");
            entity.Property(e => e.DriverId)
                .HasMaxLength(40)
                .HasColumnName("driver_id");
            entity.Property(e => e.EndPoint)
                .HasMaxLength(20)
                .HasColumnName("end_point");
            entity.Property(e => e.StartPoint)
                .HasMaxLength(20)
                .HasColumnName("start_point");
            entity.Property(e => e.TotalPlaces).HasColumnName("total_places");

            entity.HasOne(d => d.Car).WithMany(p => p.Trips)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("trip_car_id_fkey");

            entity.HasOne(d => d.Driver).WithMany(p => p.Trips)
                .HasForeignKey(d => d.DriverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("trip_driver_id_fkey");

            entity.HasOne(d => d.EndPointNavigation).WithMany(p => p.TripEndPointNavigations)
                .HasForeignKey(d => d.EndPoint)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("trip_end_point_id_fkey");

            entity.HasOne(d => d.StartPointNavigation).WithMany(p => p.TripStartPointNavigations)
                .HasForeignKey(d => d.StartPoint)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("trip_start_point_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
