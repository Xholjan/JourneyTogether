using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Journey> Journeys => Set<Journey>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Auth0Id)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasIndex(e => e.Auth0Id)
                    .IsUnique();

                entity.HasIndex(e => e.Email)
                    .IsUnique();
            });

            modelBuilder.Entity<Journey>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.StartLocation)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.ArrivalLocation)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.StartTime)
                    .IsRequired();

                entity.Property(e => e.ArrivalTime)
                    .IsRequired();

                entity.Property(e => e.TransportType)
                    .IsRequired();

                entity.Property(e => e.DistanceKm)
                    .HasColumnType("decimal(5,2)");

                entity.HasOne(j => j.User)
                      .WithMany()
                      .HasForeignKey(j => j.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}