using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TourNhanh.Models
{
    public class Transport
    {
        //Id
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //Transport name
        [Required]
        public string? Name { get;set ; }

        //Description     
        public string? Description { get; set; }
    }
}
