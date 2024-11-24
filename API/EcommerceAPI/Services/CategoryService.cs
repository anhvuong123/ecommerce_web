// Services/CategoryService.cs
using System.Collections.Generic;
using System.Linq;
using EcommerceAPI.Data;
using EcommerceAPI.Models;
using EcommerceLib.Dtos;

namespace EcommerceAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly EcommerceContext _context;

        public CategoryService(EcommerceContext context)
        {
            _context = context;
        }

        public IEnumerable<CategoryDto> GetCategories()
        {
            return _context.Categories.Select(c => new CategoryDto
            {
                CategoryId = c.Id,
                CategoryName = c.Name,
                CategoryDescription = c.Description
            }).ToList();
        }
        public CategoryDto CreateCategory(CategoryDto categoryDto)
        {
            // CategoryDto to Category entity
            var category = new Category
            {
                Name = categoryDto.CategoryName,
                Description = categoryDto.CategoryDescription
            };

            _context.Categories.Add(category);
            _context.SaveChanges();

            // Set CategoryDto with new CategoryID
            categoryDto.CategoryId = category.Id;
            return categoryDto;
        }
        public bool UpdateCategory(int id, CategoryDto categoryDto)
        {
            var existingCategory = _context.Categories.FirstOrDefault(c => c.Id == id);

            if (existingCategory == null)
                return false;

            existingCategory.Name = categoryDto.CategoryName;
            existingCategory.Description = categoryDto.CategoryDescription;

            _context.Categories.Update(existingCategory);
            _context.SaveChanges();

            return true;
        }
    }
}
