using System.ComponentModel.DataAnnotations;

namespace TodoApp.Core.DTOs.Category;

public class CreateCategoryDto
{
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }
} 