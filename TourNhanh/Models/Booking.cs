using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourNhanh.Models
{
    public class Booking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public string Note { get; set; }
        public string CustomerId { get; set; }
        public AppUser Customer { get; set; }

        public int TourId { get; set; }
        public Tour Tour { get; set; }

        public string ContactPersonId { get; set; }
        [NotMapped]
        public AppUser ContactPerson { get; set; }

        public int PaymentId { get; set; }

        public Payment Payment { get; set; }
    }


}