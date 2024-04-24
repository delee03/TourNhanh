using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TourNhanh.Models
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TourDetail>()
                .HasKey(tl => new { tl.TourId, tl.LocationId });

            /*          modelBuilder.Entity<IdentityUserLogin<string>>().HasNoKey();
                      modelBuilder.Entity<IdentityUserRole<string>>().HasNoKey();*/
        }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<TourDetail> TourDetails { get; set; }
        public DbSet<AppUser> Users { get; set; }

        public DbSet<TourImage> TourImages { get; set; }
    }
}