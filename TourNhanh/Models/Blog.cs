using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourNhanh.Models
{
    public class Blog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

     
        public string Title { get; set; }

       
        public string Content { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; } 

        public string? Author { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<Comment>? Comments { get; set; }
        public int Likes { get; set; } = 0;
        public int State { get; set; } = 0;
    }
}
