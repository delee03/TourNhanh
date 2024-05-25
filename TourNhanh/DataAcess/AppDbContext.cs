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

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Customer)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.CustomerUserId);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.ContactPerson)
                .WithMany() // không cần thuộc tính navigation property tương ứng trong AppUser
                .HasForeignKey(b => b.ContactPersonUserId);
            //comment

            /*            modelBuilder.Entity<TourDetail>()
                            .HasKey(tl => new { tl.TourId, tl.LocationId });*/

            /*          modelBuilder.Entity<IdentityUserLogin<string>>().HasNoKey();
                      modelBuilder.Entity<IdentityUserRole<string>>().HasNoKey();*/
        }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<TourDetail> TourDetails { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }

        public DbSet<TourImage> TourImages { get; set; }

        public DbSet<Transport> Transports { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Comment> comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Review> Reviews { get; set; }
    }
}