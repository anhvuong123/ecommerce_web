using Microsoft.AspNetCore.Mvc;
using EcommerceAPI.Models;
using EcommerceAPI.Services;
using EcommerceLib.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace EcommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitRating([FromBody] RatingDto ratingDto)
        {
            if (ratingDto == null || ratingDto.Score < 1 || ratingDto.Score > 5)
            {
                return BadRequest("Invalid rating data.");
            }

            var rating = new Rating
            {
                ProductId = ratingDto.ProductId,
                UserId = ratingDto.UserId,
                Score = ratingDto.Score,
                Comment = ratingDto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            var createdRating = await _ratingService.AddRatingAsync(rating);
            return CreatedAtAction(nameof(GetRatingsByProductId), new { productId = createdRating.ProductId }, createdRating);
        }

        [HttpGet("{productId}")]
        [Authorize]
        public async Task<IActionResult> GetRatingsByProductId(int productId)
        {
            var ratings = await _ratingService.GetRatingsByProductIdAsync(productId);
            return Ok(ratings);
        }
    }
}
