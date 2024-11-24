using EcommerceLib.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Challenge4.ViewComponents
{
    public class CategoryMenuViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public CategoryMenuViewComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("http://localhost:5203/api/Category"); // Đảm bảo URL này đúng

            if (!response.IsSuccessStatusCode)
            {
                return Content("Error retrieving categories.");
            }

            //var json = await response.Content.ReadAsStringAsync();
            //var categories = JsonSerializer.Deserialize<List<JsonElement>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var json = await response.Content.ReadAsStringAsync();
            var categories = JsonSerializer.Deserialize<List<CategoryDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(categories);
        }
    }
}