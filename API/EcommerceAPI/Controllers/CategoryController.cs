using Microsoft.AspNetCore.Mvc;
using EcommerceAPI.Models;
using EcommerceAPI.Data;
using Microsoft.EntityFrameworkCore;
using EcommerceAPI.Services;
using EcommerceLib.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace EcommerceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CategoryDto>> GetCategories()
        {
            var categories = _categoryService.GetCategories();
            return Ok(categories);
        }
        
        [HttpPost]
        [Authorize]
        public IActionResult CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
        {
            if (createCategoryDto == null)
                return BadRequest("Invalid category data.");

             var categoryDto = new CategoryDto // CreateCategoryDto to CategoryDto
            {
                CategoryName = createCategoryDto.CategoryName,
                CategoryDescription = createCategoryDto.CategoryDescription
            };

            var createdCategory = _categoryService.CreateCategory(categoryDto);
            return CreatedAtAction(nameof(GetCategories), new { id = createdCategory.CategoryId }, createdCategory);
        }
        
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult UpdateCategory(int id, [FromBody] CategoryDto categoryDto)
        {
            if (categoryDto == null || id != categoryDto.CategoryId)
                return BadRequest("Category ID mismatch.");

            var updated = _categoryService.UpdateCategory(id, categoryDto);
            if (!updated)
                return NotFound("Category not found.");

            return NoContent(); // Return HTTP 204 No Content if update is successful
        }
    }
}
