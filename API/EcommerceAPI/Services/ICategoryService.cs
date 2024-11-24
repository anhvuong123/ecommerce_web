using System.Collections.Generic;
using EcommerceLib.Dtos;

namespace EcommerceAPI.Services
{
    public interface ICategoryService
    {
        IEnumerable<CategoryDto> GetCategories();
        CategoryDto CreateCategory(CategoryDto categoryDto);
        bool UpdateCategory(int id, CategoryDto categoryDto);
    }
}
