using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Core.DTOs.Category;
using TodoApp.Core.Entities;
using TodoApp.Core.Interfaces;
using TodoApp.API.Controllers;

namespace TodoApp.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CategoryController : BaseController
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto dto)
    {
        var category = new Category
        {
            Name = dto.Name,
            Description = dto.Description,
            Color = dto.Color,
            UserId = int.Parse(User.Identity?.Name ?? "0")
        };

        var createdCategory = await _categoryService.CreateCategoryAsync(category);
        return CreatedAtAction(nameof(GetById), new { id = createdCategory.Id }, createdCategory);
    }

    [HttpGet]
    public async Task<IActionResult> GetUserCategories()
    {
        var userId = int.Parse(User.Identity?.Name ?? "0");
        var categories = await _categoryService.GetUserCategoriesAsync(userId);
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        return Ok(category);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] CreateCategoryDto dto)
    {
        try 
        {
         
            var existingCategory = await _categoryService.GetByIdAsync(id);
            if (existingCategory == null)
                return NotFound();

            
            existingCategory.Name = dto.Name;
            existingCategory.Description = dto.Description;
            existingCategory.Color = dto.Color;
            
          
            await _categoryService.UpdateCategoryAsync(existingCategory);
            
            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        await _categoryService.DeleteCategoryAsync(id);
        return NoContent();
    }
} 