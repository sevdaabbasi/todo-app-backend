namespace TodoApp.Core.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Color { get; set; }  // Kategorileri UI'da renklendirmek için
    
    public int UserId { get; set; }  // Her kullanıcının kendi kategorileri olabilir
    public User User { get; set; }
    
    public ICollection<Todo> Todos { get; set; }
}