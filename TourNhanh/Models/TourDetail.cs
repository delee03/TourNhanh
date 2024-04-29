using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourNhanh.Models
{
    public class TourDetail
    {
        // Id
        [Key, ForeignKey("Tour")]
        public int TourId { get; set; }

        public virtual Tour? Tour { get; set; }

        //Location
        [ForeignKey("Location")]
        public int LocationId { get; set; }
        public virtual Location? Location { get; set; }

        // Thứ tự của địa điểm trong tour.
        [Required]
        public int Order { get; set; }

        // Thời gian bắt đầu tại địa điểm.
        [DataType(DataType.Time)]
        public DateTime StartTime { get; set; }

        // Thời gian kết thúc tại địa điểm.
        [DataType(DataType.Time)]
        public DateTime EndTime { get; set; }

        // Id của khách sạn (Hotel).
        [ForeignKey("Hotel")]
        public int HotelId { get; set; }

        // Đối tượng Hotel tương ứng.
        public virtual Hotel? Hotel { get; set; }
    }
}
