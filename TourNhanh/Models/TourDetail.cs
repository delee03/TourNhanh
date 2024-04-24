using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourNhanh.Models
{
    public class TourDetail
    {
        public int TourId { get; set; }
        public Tour Tour { get; set; }

        public int LocationId { get; set; }
        public Location Location { get; set; }

        public int Order { get; set; }

        [DataType(DataType.Time)]
        public DateTime StartTime { get; set; }

        [DataType(DataType.Time)]
        public DateTime EndTime { get; set; }

        [ForeignKey("Hotel")]
        public int HotelId { get; set; }

        public virtual Hotel? Hotel { get; set; }
    }

}
