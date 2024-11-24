using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EcommerceLib.Dtos;
using System.Text.Json;

namespace eCommerce_Practice.Controllers
{
    public class ProductController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ProductController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> GetProductByCategory(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"http://localhost:5203/api/Product?categoryId={id}"); // Đảm bảo URL này đúng

            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Error", "Home");
            }

            var json = await response.Content.ReadAsStringAsync();
            var products = JsonSerializer.Deserialize<List<ProductDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(products);
        }    

        public async Task<IActionResult> GetProductDetails(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"http://localhost:5203/api/Product/{id}"); 

            var json = await response.Content.ReadAsStringAsync();
            var product = JsonSerializer.Deserialize<ProductDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(product); // Trả về View với sản phẩm chi tiết
        }

        public async Task<IActionResult> GetProducts(int pageNumber = 1, int pageSize = 10)
        {
            var client = _httpClientFactory.CreateClient();

            // Gọi API để lấy dữ liệu sản phẩm với phân trang
            var response = await client.GetAsync($"http://localhost:5203/api/Product/pagination?pageNumber={pageNumber}&pageSize={pageSize}");

            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Error", "Home");
            }

            var json = await response.Content.ReadAsStringAsync();

            var pagedResult = JsonSerializer.Deserialize<JsonElement>(json);

            if (pagedResult.TryGetProperty("items", out var itemsProperty))
            {
                var products = JsonSerializer.Deserialize<List<ProductDto>>(itemsProperty.ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                var totalCount = 0;
                var totalPages = 0;

                if (pagedResult.TryGetProperty("totalCount", out var totalCountProperty))
                {
                    totalCount = totalCountProperty.GetInt32();
                }

                // Kiểm tra nếu trường "TotalPages" tồn tại
                if (pagedResult.TryGetProperty("totalPages", out var totalPagesProperty))
                {
                    totalPages = totalPagesProperty.GetInt32();
                }

                // Truyền thông tin phân trang vào ViewBag
                ViewBag.TotalPages = totalPages;
                ViewBag.CurrentPage = pageNumber;

                // Trả về View với danh sách sản phẩm
                return View(products);
            }
            else
            {
                return BadRequest("Can't find the node 'items' from JSON.");
            }
        }

    }
}
