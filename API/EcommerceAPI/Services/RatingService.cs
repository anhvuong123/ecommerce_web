using EcommerceAPI.Data;
using EcommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Services
{
    public class RatingService : IRatingService
    {
        private readonly EcommerceContext _context;

        public RatingService(EcommerceContext context)
        {
            _context = context;
        }

        public async Task<Rating> AddRatingAsync(Rating rating)
        {
            rating.CreatedAt = DateTime.UtcNow;
            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();
            // Cập nhật thông tin của Product sau khi thêm rating
            var product = await _context.Products
                .Include(p => p.Ratings) // Include ratings để dễ dàng tính toán trung bình
                .FirstOrDefaultAsync(p => p.Id == rating.ProductId);

            if (product != null)
            {
                // Tính toán lại trung bình rating
                var averageRating = product.Ratings.Average(r => r.Score);
                var ratingCount = product.Ratings.Count;

                // Cập nhật lại giá trị ProductAverageRating và ProductRatingCount
                product.AverageRating = (decimal?)averageRating;
                product.RatingCount = ratingCount;

                // Lưu thay đổi
                await _context.SaveChangesAsync();
            }
            return rating;
        }

        public async Task<IEnumerable<Rating>> GetRatingsByProductIdAsync(int productId)
        {
            return await _context.Ratings
                .Where(r => r.ProductId == productId)
                .ToListAsync();
        }
    }
}
