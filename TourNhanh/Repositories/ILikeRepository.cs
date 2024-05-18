using TourNhanh.Models;

namespace TourNhanh.Repositories
{
    public interface ILikeRepository
    {
        Task<IEnumerable<Like>> GetAllAsync();
        Task<Like> HasLikedAsync(string userId, int blogId);
        Task AddAsync(Like like);
        Task RemoveAsync(string userId, int blogId);
        Task UpdateAsync(Like like);
    }

}
