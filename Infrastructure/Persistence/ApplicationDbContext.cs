using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Journey> Journeys => Set<Journey>();
        public DbSet<Share> Shares => Set<Share>();
        public DbSet<PublicLink> PublicLinks => Set<PublicLink>();
        public DbSet<Audit> Audits => Set<Audit>();

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
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Share>()
                .HasOne(s => s.Journey)
                .WithMany()
                .HasForeignKey(s => s.JourneyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Share>()
                .HasOne(s => s.SharedBy)
                .WithMany()
                .HasForeignKey(s => s.SharedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Share>()
                .HasOne(s => s.SharedWith)
                .WithMany()
                .HasForeignKey(s => s.SharedWithUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PublicLink>()
                .HasOne(p => p.Journey)
                .WithMany()
                .HasForeignKey(p => p.JourneyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Audit>()
                .HasOne(a => a.Journey)
                .WithMany()
                .HasForeignKey(a => a.JourneyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Audit>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}