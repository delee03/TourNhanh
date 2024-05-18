using TourNhanh.Models;

namespace TourNhanh.Repositories.Interfaces
{
    public interface ITransportRepository
    {
        Task<IEnumerable<Transport>> GetAllAsync();
        Task<Transport?> GetByIdAsync(int id);
        Task CreateAsync(Transport transport);
        Task UpdateAsync(Transport transport);
        Task DeleteAsync(int id);
    }
}
