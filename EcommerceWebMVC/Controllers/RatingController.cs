using System.Net.Http;
using System.Net.Http.Json; // Để sử dụng JsonContent
using EcommerceLib.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce_Practice.Controllers
{
    public class RatingController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RatingController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitRating(int productId, int score, string comment)
        {
            var ratingRequest = new RatingDto
            {
                ProductId = productId,
                Score = score,
                Comment = comment,
                CreatedAt = DateTime.UtcNow
            };

            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync("http://localhost:5203/api/rating", ratingRequest);

            return RedirectToAction("GetProductDetails", "Product", new { id = productId });
        }
    }
}
