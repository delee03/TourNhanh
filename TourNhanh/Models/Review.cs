namespace TourNhanh.Models
{
	public class Review
	{
		public int Id { get; set; }

		public string Content { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		public string? Author { get; set; }

		public int TourId { get; set; }	
		public Tour Tour { get; set; }
		public int Rating { get; set; } = 0;
		public string? Email { get; set; }
	}
}
