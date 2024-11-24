using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EcommerceAPI.Models;
using Microsoft.EntityFrameworkCore;
using EcommerceAPI.Services;
using EcommerceLib.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace EcommerceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly string _uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

        public ProductController(IProductService productService)
        {
            _productService = productService;
            if (!Directory.Exists(_uploadFolder))
            {
                Directory.CreateDirectory(_uploadFolder);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductDto createProductDto)
        {
            if (createProductDto == null)
            {
                return BadRequest("Product data is null.");
            }
          
            var productDto = new ProductDto // CreateProductDto to ProductDto
            {
                ProductName = createProductDto.ProductName,
                ProductDescription = createProductDto.ProductDescription,
                ProductPrice = createProductDto.ProductPrice,
                CategoryId = createProductDto.CategoryId,
                ProductNote = createProductDto.ProductNote,
                ProductImageUrl = createProductDto.ProductImageUrl
            };

            var createdProduct = await _productService.CreateProduct(productDto);

            return Ok(createdProduct);
        }

        [HttpPost("upload")]
        [Authorize]
        public async Task<IActionResult> UploadImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            // Define the path to save the file
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", image.FileName);

            // Ensure the directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            // Save the file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return Ok(new { FilePath = filePath });
        }
      
        [HttpGet]
        public ActionResult<IEnumerable<ProductDto>> GetProductsByCategory(int categoryId)
        {
            var products = _productService.GetProductsByCategory(categoryId);
            return Ok(products);
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<ProductDto>> GetProductDetails(int productId)
        {
            var product = await _productService.GetProductById(productId);            
            return Ok(product);
        }

        [HttpGet("pagination")]
        public ActionResult<object> GetProducts(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                throw new ArgumentException("Page number and page size must be greater than 0.");
            }

            // Get products by ProductService <pageNumber, pageSize>
            var productDtos = _productService.GetProducts(pageNumber, pageSize);

            // Get the total number of products by ProductService
            var totalCount = _productService.GetTotalProductCount();

            // Calculate the total number of pages
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return Ok(new
            {
                Items = productDtos,
                TotalCount = totalCount,
                TotalPages = totalPages
            });
        }

        [HttpPut("{productId}")]
        [Authorize]
        public async Task<ActionResult<ProductDto>> UpdateProduct(int productId, [FromBody] UpdateProductDto updateProductDto)
        {
            if (updateProductDto == null)
            {
                return BadRequest("Product data is null.");
            }

            var productDto = new ProductDto // CreateProductDto to ProductDto
            {
                ProductName = updateProductDto.ProductName,
                ProductDescription = updateProductDto.ProductDescription,
                ProductPrice = updateProductDto.ProductPrice,
                CategoryId = updateProductDto.CategoryId,
                ProductNote = updateProductDto.ProductNote,
                ProductAverageRating = updateProductDto.ProductAverageRating,
                ProductRatingCount = updateProductDto.ProductRatingCount,
                ProductImageUrl = updateProductDto.ProductImageUrl
            };

            var updatedProduct = await _productService.UpdateProduct(productId, productDto);
            return Ok(updatedProduct);
        }

        [HttpDelete("{productId}")]
        [Authorize]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var result = await _productService.DeleteProduct(productId);
            
            if (!result)
            {
                return NotFound(); // has no product to remove
            }

            return NoContent(); // remove successfully
        }
    }
}