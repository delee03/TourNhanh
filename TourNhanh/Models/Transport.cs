using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Net;

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


        public List<string> SplitTransport()
        {
            return Name?.Split(',').ToList() ?? new List<string>();
        }


        //Description     
        public string? Description { get; set; }
    }
}
