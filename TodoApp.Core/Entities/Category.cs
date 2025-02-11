namespace TodoApp.Core.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Color { get; set; }  
    
    public int UserId { get; set; }  
    public User User { get; set; }
    
    public ICollection<Todo> Todos { get; set; }
}