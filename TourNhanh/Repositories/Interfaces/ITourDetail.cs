using TourNhanh.Models;

namespace TourNhanh.Repositories.Interfaces
{
    public interface ITourDetail
    {
        Task<IEnumerable<TourDetail>> GetAllAsync();
        Task<TourDetail?> GetByIdAsync(int id);
        Task<IEnumerable<TourDetail>> GetByTourIdAsync(int tourId);
        Task CreateAsync(TourDetail tourDetail);
        Task UpdateAsync(TourDetail tourDetail);
        Task DeleteAsync(int id);
        Task DeleteByTourIdAsync(int tourId);
    }
}
