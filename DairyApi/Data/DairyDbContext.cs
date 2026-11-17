using Microsoft.EntityFrameworkCore;
using DairyApi.Models;

namespace DairyApi.Data
{
    public class DairyDbContext : DbContext
    {
        public DairyDbContext(DbContextOptions<DairyDbContext> options)
            : base(options)
        {
        }

        public DbSet<Farmer> Farmers { get; set; }
        public DbSet<MilkDelivery> MilkDeliveries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Primary key is automatically DeliveryId for MilkDelivery
            modelBuilder.Entity<MilkDelivery>()
                        .HasKey(m => m.DeliveryId);

            // Farmer unique NationalId
            modelBuilder.Entity<Farmer>()
                        .HasIndex(f => f.NationalId)
                        .IsUnique();

            // One-to-many relationship
            modelBuilder.Entity<MilkDelivery>()
                        .HasOne<Farmer>()
                        .WithMany(f => f.MilkDeliveries)
                        .HasForeignKey(m => m.FarmerId)
                        .OnDelete(DeleteBehavior.Cascade);

            // Check constraint
            modelBuilder.Entity<MilkDelivery>()
                        .HasCheckConstraint("CK_Quantity_Positive", "QuantityLitres > 0");
        }
    }
}
