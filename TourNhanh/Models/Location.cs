using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourNhanh.Models
{
    public class Location
    {
        // Khóa chính, tự động tăng.
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Tên của địa điểm, không được để trống và tối đa 100 ký tự.
        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        // Mô tả về địa điểm, tối đa 500 ký tự.
        [StringLength(500)]
        public string? Description { get; set; }

        // Tọa độ vĩ độ của địa điểm.
        public double? Latitude { get; set; }

        // Tọa độ kinh độ của địa điểm.
        public double? Longitude { get; set; }

        public string? Address { get; set; }

        // Hàm để cắt chuỗi Address
        public List<string> SplitAddress ()
        {
            return Address.Split('-').ToList();
        
        }
      
    }
}
