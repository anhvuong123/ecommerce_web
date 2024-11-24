// Services/CategoryService.cs
using System.Collections.Generic;
using System.Linq;
using EcommerceAPI.Data;
using EcommerceAPI.Models;
using EcommerceLib.Dtos;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly EcommerceContext _context;

        public ProductService(EcommerceContext context)
        {
            _context = context;
        }

        public async Task<ProductDto> CreateProduct(ProductDto productDto)
        {
            // ProductDto to Product entity
            var product = new Product
            {
                Name = productDto.ProductName,
                Description = productDto.ProductDescription,
                Price = productDto.ProductPrice,
                ImageUrl = productDto.ProductImageUrl,
                Note = productDto.ProductNote,
                CategoryId = productDto.CategoryId, // Giả sử CategoryId được cung cấp
                CreatedDate = DateTime.UtcNow
            };
            var category = await _context.Categories.FindAsync(productDto.CategoryId);
            product.Category = category;

            // Add product into DB
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Product entity to ProductDto for return
            return new ProductDto
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ProductDescription = product.Description,
                ProductPrice = product.Price,
                ProductImageUrl = product.ImageUrl,
                ProductAverageRating = product.AverageRating,
                ProductRatingCount = product.RatingCount,
                CategoryId = (int)product.CategoryId,
                Category = new CategoryDto
                {
                    CategoryId = product.Category.Id,
                    CategoryName = product.Category.Name,
                    CategoryDescription = product.Category.Description
                },
                ProductNote = product.Note,
                CreatedDate = product.CreatedDate
            };
        }


        public IEnumerable<ProductDto> GetProductsByCategory(int categoryId)
        {
            var products = _context.Products
                .Where(p => p.CategoryId == categoryId)
                .Include(p => p.Category) // inlcude Category infomation
                .ToList();

            return products.Select(p => new ProductDto
            {
                ProductId = p.Id,
                ProductName = p.Name,
                ProductDescription = p.Description,
                ProductPrice = p.Price,
                ProductImageUrl = p.ImageUrl,
                ProductAverageRating = p.AverageRating,
                ProductRatingCount = p.RatingCount,
                Category = new CategoryDto
                {
                    CategoryId = p.Category.Id,
                    CategoryName = p.Category.Name
                }
            }).ToList();
        }
        public async Task<ProductDto> GetProductById(int id)
        {
                var product = await _context.Products
                .Include(p => p.Category) // inlcude Category infomation
                .Include(p => p.Ratings) // inlcude Rating infomation
                .FirstOrDefaultAsync(p => p.Id == id);

                if (product == null) return null;

            return new ProductDto
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ProductDescription = product.Description,
                ProductPrice = product.Price,
                ProductImageUrl = product.ImageUrl,
                ProductNote = product.Note,
                ProductAverageRating = product.AverageRating,
                ProductRatingCount = product.RatingCount,
                CategoryId = (int)product.CategoryId,
                Category = new CategoryDto // convert to CategoryDto
                {
                    CategoryId = product.Category.Id,
                    CategoryName = product.Category.Name,
                    CategoryDescription = product.Category.Description
                },
                Ratings = product.Ratings?.Select(r => new RatingDto
                {
                    Score = r.Score,
                    Comment = r.Comment
                }).ToList() ?? new List<RatingDto>()
            };
        }

        public int GetTotalProductCount()
        {
            return _context.Products.Count();
        }

        public IEnumerable<ProductDto> GetProducts(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                throw new ArgumentException("Page number and page size must be greater than 0.");
            }

            var totalCount = _context.Products.Count();

            // Get product by specific page
            var products = _context.Products
                .Include(p => p.Category)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // product entity to ProductDto for return
            var productDtos = products.Select(p => new ProductDto
            {
                ProductId = p.Id,
                ProductName = p.Name,
                ProductDescription = p.Description,
                ProductPrice = p.Price,
                ProductImageUrl = p.ImageUrl,
                ProductAverageRating = p.AverageRating,
                ProductRatingCount = p.RatingCount,
                Category = new CategoryDto
                {
                    CategoryId = p.Category.Id,
                    CategoryName = p.Category.Name,
                    CategoryDescription = p.Category.Description
                },
                CreatedDate = p.CreatedDate,
                UpdatedDate = p.UpdatedDate
            }).ToList();
            return productDtos;
        }

        public async Task<ProductDto> UpdateProduct(int productId, ProductDto productDto)
        {
            var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == productId);
            
            if (product == null) return null;

            // update product from productDto
            product.Name = productDto.ProductName;
            product.Description = productDto.ProductDescription;
            product.Price = productDto.ProductPrice;
            product.ImageUrl = productDto.ProductImageUrl;
            product.AverageRating = productDto.ProductAverageRating;
            product.RatingCount = productDto.ProductRatingCount;
            product.Note = productDto.ProductNote;
            // Cập nhật thông tin danh mục nếu cần
            if (product.CategoryId != productDto.CategoryId)
            {
                var category = await _context.Categories.FindAsync(productDto.CategoryId);
                if (category != null)
                {
                    product.CategoryId = productDto.CategoryId; 
                    product.Category = category;
                }
            }
            product.UpdatedDate = DateTime.UtcNow;

            // update DB
            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            // Product entity to ProductDto for return
            return new ProductDto
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ProductDescription = product.Description,
                ProductPrice = product.Price,
                ProductImageUrl = product.ImageUrl,
                ProductAverageRating = product.AverageRating, 
                ProductRatingCount = product.RatingCount,
                CategoryId = (int)product.CategoryId,
                Category = new CategoryDto
                {
                    CategoryId = product.Category.Id,
                    CategoryName = product.Category.Name,
                    CategoryDescription = product.Category.Description
                },
                Ratings = product.Ratings.Select(r => new RatingDto
                {
                    Score = r.Score,
                    Comment = r.Comment
                }).ToList(),
                CreatedDate = product.CreatedDate,
                UpdatedDate = product.UpdatedDate,
                ProductNote = product.Note
                
            };
        }
        public async Task<bool> DeleteProduct(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            
            return true;
        }
    }
}
