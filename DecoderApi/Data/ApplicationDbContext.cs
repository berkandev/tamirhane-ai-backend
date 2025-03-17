using DecoderApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DecoderApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Ecu> Ecus { get; set; }
        public DbSet<TuneCatalog> TuneCatalogs { get; set; }
        public DbSet<DiffModification> DiffModifications { get; set; }
        public DbSet<ApiLog> ApiLogs { get; set; }
        public DbSet<License> Licenses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User entity konfigürasyonu
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Vehicle entity konfigürasyonu
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.User)
                .WithMany(u => u.Vehicles)
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Ecu entity konfigürasyonu
            modelBuilder.Entity<Ecu>()
                .HasOne(e => e.Vehicle)
                .WithMany(v => v.Ecus)
                .HasForeignKey(e => e.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            // TuneCatalog entity konfigürasyonu
            modelBuilder.Entity<TuneCatalog>()
                .HasOne(t => t.Ecu)
                .WithMany(e => e.TuneCatalogs)
                .HasForeignKey(t => t.EcuId)
                .OnDelete(DeleteBehavior.Cascade);

            // DiffModification entity konfigürasyonu
            modelBuilder.Entity<DiffModification>()
                .HasOne(d => d.TuneCatalog)
                .WithMany(t => t.DiffModifications)
                .HasForeignKey(d => d.TuneCatalogId)
                .OnDelete(DeleteBehavior.Cascade);

            // DiffModification için JSONB yapılandırması
            modelBuilder.Entity<DiffModification>()
                .Property(d => d.OriginalDataJson)
                .HasColumnType("jsonb");

            modelBuilder.Entity<DiffModification>()
                .Property(d => d.ModifiedDataJson)
                .HasColumnType("jsonb");

            // License entity konfigürasyonu
            modelBuilder.Entity<License>()
                .HasOne(l => l.User)
                .WithMany(u => u.Licenses)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}