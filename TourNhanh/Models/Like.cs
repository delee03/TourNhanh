namespace TourNhanh.Models
{
    public class Like
    {
        public int LikeId { get; set; }
        public Blog Blog { get; set; }
        public int BlogId { get; set; }
        public string UserId { get; set; }
        public bool Liked { get; set; } = false;
    }

}
