using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TourNhanh.Models;
using TourNhanh.Repositories.Interfaces;

namespace TourNhanh.Repositories.Implementations
{
	public class EFReviewRepository : IReviewRepository
	{
		private readonly AppDbContext _context;

		public EFReviewRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Review>> GetAllAsync()
		{
			return await _context.Reviews.Reverse().ToListAsync();
		}

		public async Task<Review> GetByIdAsync(int id)
		{
			return await _context.Reviews.FindAsync(id);
		}

		public async Task<List<Review>> GetReviewsByTourId(int tourId)
		{
			return await _context.Reviews
				.Where(r => r.TourId == tourId)
				.ToListAsync();
		}

		public async Task AddAsync(Review review)
		{
			_context.Reviews.Add(review);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateAsync(Review review)
		{
			_context.Reviews.Update(review);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var review = await _context.Reviews.FindAsync(id);
			_context.Reviews.Remove(review);
			await _context.SaveChangesAsync();
		}

		public async Task<List<Review>> GetByAuthorAsync(string author)
		{
			return await _context.Reviews.Where(r => r.Author == author).ToListAsync();
		}

		public async Task<List<Review>> GetByRatingAsync(int rating)
		{
			return await _context.Reviews.Where(r => r.Rating == rating).ToListAsync();
		}
	}
}
