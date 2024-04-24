
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourNhanh.Models
{
    public class Tour
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        /*        [ForeignKey("Location")]
                public int StartLocationId { get; set; }
                public Location StartLocation { get; set; }

                [ForeignKey("Location")]
                public int EndLocationId { get; set; }
                public Location EndLocation { get; set; }*/

        public int CategoryId { get; set; } // ID của danh mục
        public Category Category { get; set; } // Danh mục của tour

        public List<TourDetail> TourDetails { get; set; }
    }


}
