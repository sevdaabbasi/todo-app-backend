namespace TodoApp.Core.Entities;

public class Category: BaseEntity
{
    public string Name { get; set; }
    
  
    
    public ICollection<Todo> Todos { get; set; }
}