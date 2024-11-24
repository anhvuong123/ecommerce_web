using EcommerceAPI.Models;

namespace EcommerceAPI.Services
{
    public interface IRatingService
    {
        Task<Rating> AddRatingAsync(Rating rating);
        Task<IEnumerable<Rating>> GetRatingsByProductIdAsync(int productId);
    }
}
