using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourNhanh.Models
{
    public class Booking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //Booking date
        [DataType(DataType.Date)]
        public DateTime BookingDate { get; set; }

        //Customer
        [ForeignKey("Customer")]
        public string? CustomerUserId { get; set; }
        public virtual AppUser? Customer { get; set; }
        //Quantity
        public int? Quantity { get; set; }

        //Tour
        public int TourId { get; set; }
        public virtual Tour? Tour { get; set; }

        //ContactPerson
        [ForeignKey("ContactPerson")]
        public string? ContactPersonUserId { get; set; }
        public virtual AppUser? ContactPerson { get; set; }

        //Note
        public string? Note { get; set; }
        //Payment
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [DataType(DataType.Date)]
        public DateTime? PaymentDate { get; set; }

        public string? PaymentMethod { get; set; }

        public bool? isPaymentCompleted { get; set; }
    }

}
