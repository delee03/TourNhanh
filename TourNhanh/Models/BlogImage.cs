namespace TourNhanh.Models
{
    public class BlogImage
    {
        public int Id { get; set; }
        public string Ulr { get; set; }
        public int BlogId { get; set; }
        public Blog? Blog { get; set; }
    }
}
