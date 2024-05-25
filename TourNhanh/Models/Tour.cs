
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourNhanh.Models
{
    public class Tour
    {
        //ID
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //Category
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }

        //Name
        [Required]   
        public string? Name { get; set; }

        //Description   
        public string? Description { get; set; }

        //Price
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        //Transport
        [ForeignKey("Transport")]
        public int TransportId { get; set; }
        public virtual Transport? Transport { get; set; }

        //MainImage
        public string? MainImageUrl { get; set; }

        //ListImage
        public virtual ICollection<TourImage>? TourImages{get;set;}

        //Detail
        public virtual ICollection<TourDetail>? TourDetails { get; set; }
    }
}
