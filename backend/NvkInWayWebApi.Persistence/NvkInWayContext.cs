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
        Database.EnsureCreated();
    }

    public virtual DbSet<CarEntity> Cars { get; set; }

    public virtual DbSet<DriverEntity> Drivers { get; set; }

    public virtual DbSet<LocationEntity> Locations { get; set; }

    public virtual DbSet<PassengerEntity> Passengers { get; set; }

    public virtual DbSet<RecordEntity> Records { get; set; }

    public virtual DbSet<TaxisEntity> Taxes { get; set; }

    public virtual DbSet<TripEntity> Trips { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CarEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cars_pkey");

            entity.ToTable("cars");

            entity.Property(e => e.Color)
                .HasMaxLength(20)
                .HasColumnName("color");
            entity.Property(e => e.DriverId)
                .HasMaxLength(40)
                .HasColumnName("driver_id");
            entity.Property(e => e.Name)
                .HasMaxLength(40)
                .HasColumnName("name");
            entity.Property(e => e.Number)
                .HasMaxLength(9)
                .HasColumnName("number");

            entity.HasOne(d => d.Driver).WithMany(p => p.Cars)
                .HasForeignKey(d => d.DriverId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("car_driver_id_fkey");
        });

        modelBuilder.Entity<DriverEntity>(entity =>
        {
            entity.HasKey(e => e.TgProfileId).HasName("driver_id_pkey");

            entity.ToTable("drivers");

            entity.Property(e => e.AllTripsCount).HasColumnName("all_trips_count");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.TgProfileId).HasColumnName("tg_profile_id");
        });

        modelBuilder.Entity<LocationEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("locations_pkey");

            entity.ToTable("locations");

            entity.Property(e => e.Latitude).HasColumnName("latitude");
            entity.Property(e => e.Longitude).HasColumnName("longitude");
            entity.Property(e => e.Description).HasColumnName("description");
        });

        modelBuilder.Entity<PassengerEntity>(entity =>
        {
            entity.HasKey(e => e.TgProfileId).HasName("tg_profile_id");

            entity.ToTable("passengers");

            entity.Property(e => e.Rating).HasColumnName("rating");
            
            entity.Property(e => e.TripCount).HasColumnName("trip_count");
        });

        modelBuilder.Entity<RecordEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("claims_pkey");

            entity.ToTable("records");

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

        modelBuilder.Entity<TaxisEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("taxis_pkey");

            entity.ToTable("taxis");

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

        modelBuilder.Entity<TripEntity>(entity =>
        {
            // Установка первичного ключа
            entity.HasKey(e => e.Id)
                .HasName("trips_pkey");

            // Настройка таблицы
            entity.ToTable("trips");

            // Настройка свойств
            entity.Property(e => e.DriverId)
                .HasColumnName("driver_id")
                .IsRequired(); // Обязательно

            entity.Property(e => e.CarId)
                .HasColumnName("car_id")
                .IsRequired(); // Обязательно

            entity.Property(e => e.StartPointId)
                .HasColumnName("startpoint_id")
                .IsRequired(); // Обязательно

            entity.Property(e => e.EndPointId)
                .HasColumnName("endpoint_id")
                .IsRequired(); // Обязательно

            entity.Property(e => e.CarLocation)
                .HasColumnName("carlocation")
                .HasMaxLength(255); // Максимальная длина строки

            entity.Property(e => e.DriveStartTime)
                .HasColumnName("drive_start_time");

            entity.Property(e => e.DriveEndTime)
                .HasColumnName("drive_end_time")
                .IsRequired(); // Обязательно

            entity.Property(e => e.TotalPlaces)
                .HasColumnName("total_places")
                .IsRequired(); // Обязательно

            entity.Property(e => e.BookedPlaces)
                .HasColumnName("booked_places")
                .IsRequired(); // Обязательно

            //Тут собака зарыта

            // Настройка отношений с другими сущностями
            entity.HasOne(d => d.Car)
                .WithMany(c => c.Trips) // Предполагается, что у Car нет навигационного свойства для поездок
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull) // Поведение при удалении
                .HasConstraintName("fk_trips_car");

            entity.HasOne(d => d.Driver)
                .WithMany(c => c.Trips) // Предполагается, что у Driver нет навигационного свойства для поездок
                .HasForeignKey(d => d.DriverId)
                .OnDelete(DeleteBehavior.Cascade) // Поведение при удалении
                .HasConstraintName("fk_trips_driver");

            entity.HasOne(d => d.StartPointNavigation)
                .WithMany(c => c.TripStartPointNavigations) // Предполагается, что у Location нет навигационного свойства для поездок
                .HasForeignKey(d => d.StartPointId)
                .OnDelete(DeleteBehavior.ClientSetNull) // Поведение при удалении
                .HasConstraintName("fk_trips_start_point");

            entity.HasOne(d => d.EndPointNavigation)
                .WithMany(c => c.TripEndPointNavigations) // Предполагается, что у Location нет навигационного свойства для поездок
                .HasForeignKey(d => d.EndPointId)
                .OnDelete(DeleteBehavior.ClientSetNull) // Поведение при удалении
                .HasConstraintName("fk_trips_end_point");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
