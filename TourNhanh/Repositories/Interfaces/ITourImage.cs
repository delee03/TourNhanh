using TourNhanh.Models;

namespace TourNhanh.Repositories.Interfaces
{
    public interface ITourImage
    {
        Task<IEnumerable<TourImage>> GetAllAsync();
        Task<TourImage?> GetByIdAsync(int id);
        Task<IEnumerable<TourImage>> GetByTourIdAsync(int tourId);
        Task CreateAsync(TourImage tourImage);
        Task UpdateAsync(TourImage tourImage);
        Task DeleteAsync(int id);
    }
}
