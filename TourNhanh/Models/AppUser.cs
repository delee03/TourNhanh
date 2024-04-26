using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TourNhanh.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }     
        public int Age { get; set; }
        public string? Address {  get; set; }
        public List<Booking> Bookings { get; set; }
    }
}