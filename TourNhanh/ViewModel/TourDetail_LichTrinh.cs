using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TourNhanh.Models;

namespace TourNhanh.ViewModel
{
    public class TourDetail_LichTrinh
    {
		// Tour properties
		public int Id { get; set; }
		public int CategoryId { get; set; }
		public Category? Category { get; set; }
		public string? Name { get; set; }
		public string? Description { get; set; }
		public decimal Price { get; set; }
		public int TransportId { get; set; }
		public Transport? Transport { get; set; }
		public string? MainImageUrl { get; set; }
		public ICollection<TourImage>? TourImages { get; set; }

		// TourDetail properties
		public int TourDetailId { get; set; }
		public int TourId { get; set; }
		public Tour? Tour { get; set; }
		public int LocationId { get; set; }
		public Location? Location { get; set; }
		public int Order { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public int HotelId { get; set; }
		public Hotel? Hotel { get; set; }
	}
}
