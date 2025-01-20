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
        public string? AIPreferences { get; set; }
        public string? DailyRoutine { get; set; }
        public ICollection<Todo> Todos { get; set; }
        public ICollection<Plan> Plans { get; set; }
        public ICollection<TodoCollaborator> CollaborativeTodos { get; set; }
        public ICollection<PlanCollaborator> CollaborativePlans { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
}