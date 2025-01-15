namespace TodoApp.Core.Entities
{
    public class User : BaseEntity
    {
       
        //public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }  
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; } 
        public string Gender { get; set; }
        public ICollection<Todo> Todos { get; set; }
    }
}