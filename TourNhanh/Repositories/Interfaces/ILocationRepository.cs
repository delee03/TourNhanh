using TourNhanh.Models;

namespace TourNhanh.Repositories.Interfaces
{
    public interface ILocationRepository
    {
        Task<IEnumerable<Location>> GetAllAsync();
        Task<Location?> GetByIdAsync(int id);
        Task CreateAsync(Location location);
        Task UpdateAsync(Location location);
        Task DeleteAsync(int id);
    }
}
