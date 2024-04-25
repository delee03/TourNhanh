using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourNhanh.Models
{
    public class TourImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Tour")]
        public int TourId { get;set; }
        public virtual Tour? Tour { get; set; }

        public string? ImageUrl { get; set; }
    }
}
