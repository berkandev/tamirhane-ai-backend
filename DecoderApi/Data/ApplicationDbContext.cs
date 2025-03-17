using Microsoft.EntityFrameworkCore;
using DecoderApi.Models;

namespace DecoderApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<License> Licenses { get; set; } = null!;
        public DbSet<Vehicle> Vehicles { get; set; } = null!;
        public DbSet<EcuModel> EcuModels { get; set; } = null!;
        public DbSet<VehicleEcuModel> VehicleEcuModels { get; set; } = null!;
        public DbSet<TuneCatalog> TuneCatalogs { get; set; } = null!;
        public DbSet<DiffModification> DiffModifications { get; set; } = null!;
        public DbSet<ApiLog> ApiLogs { get; set; } = null!;
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Kullanıcı adı ve e-posta benzersiz olmalı
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
                
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
                
            // Lisans anahtarı benzersiz olmalı
            modelBuilder.Entity<License>()
                .HasIndex(l => l.LicenseKey)
                .IsUnique();
                
            // Araç-ECU ilişkisi için bileşik anahtar
            modelBuilder.Entity<VehicleEcuModel>()
                .HasIndex(ve => new { ve.VehicleId, ve.EcuModelId })
                .IsUnique();
        }
    }
} 