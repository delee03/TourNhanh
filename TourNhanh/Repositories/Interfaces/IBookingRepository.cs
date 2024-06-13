using TourNhanh.Models;

namespace TourNhanh.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        Task<IEnumerable<Booking>> GetAllAsync();
        Task<Booking?> GetByIdAsync(int id);
        Task CreateAsync(Booking booking);
        Task UpdateAsync(Booking booking);
        Task DeleteAsync(int id);
        Task<int> GetTotalBookingsCountAsync();
        Task<decimal> GetTotalRevenueAsync();
        Task<IEnumerable<Booking>> GetUserTour(string userId);
    }
}
