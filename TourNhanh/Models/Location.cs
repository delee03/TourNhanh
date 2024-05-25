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
        public string? Name { get; set; }

        // Mô tả về địa điểm, tối đa 500 ký tự.
        public string?Description { get; set; }
        public List<string> SplitDescription()
        {
            return Description.Split('-').ToList();
        }    
        public string? Desc1 { get; set; }
       
        public string? Desc2 { get; set; }
       
        public string? Desc3 { get; set; }
        
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        //gồm 3 địa điểm tối đa cách nhau bởi dấu " - ";
        public string? Address { get; set; }

        // Hàm để cắt chuỗi Address-
        public List<string> SplitAddress ()
        {
            return Address.Split('-').ToList();
        
        }
      
    }
}
