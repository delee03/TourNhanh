using System.ComponentModel.DataAnnotations;

namespace TourNhanh.Models
{
    public class LocationViewModel
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public string? Description { get; set; }

        public List<string> Addresses { get; set; } = new List<string> { "" };

        public List<string> Descriptions { get; set; } = new List<string> { "" };

        public float? Longitude { get; set; }

        public float? Latitude { get; set; }
    }
}