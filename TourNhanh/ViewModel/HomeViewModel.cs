using TourNhanh.Models;

namespace TourNhanh.ViewModel
{
    public class HomeViewModel
    {
        public IEnumerable<Tour> ToursByRating { get; set; }
        public IEnumerable<Tour> ToursByPopularity { get; set; }
        public IEnumerable<Tour> ToursByNewest { get; set; }
    }

}
