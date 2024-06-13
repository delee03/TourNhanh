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

        // Tên của địa điểm, không được để trống
        [Required]  
        public string? Name { get; set; }

        // Mô tả về địa điểm
        public string?Description { get; set; }
        public List<string> SplitDescription()
        {
            return Description?.Split('-').ToList() ?? new List<string>();
        }    
        public string? Desc1 { get; set; }
       
        public string? Desc2 { get; set; }
       
        public string? Desc3 { get; set; }
        
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        // Địa chỉ gồm 3 địa điểm tối đa cách nhau bởi dấu "-"
        public string? Address { get; set; }

        public List<string> SplitAddress()
        {
            return Address?.Split('-').ToList() ?? new List<string>();
        }

    }
}
