using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TodoApp.Core.Entities;

namespace TodoApp.Repositories
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Todo> Todos { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<TodoCollaborator> TodoCollaborators { get; set; }
        public DbSet<PlanCollaborator> PlanCollaborators { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Todo>()
                .HasOne(t => t.User)
                .WithMany(u => u.Todos)
                .HasForeignKey(t => t.UserId);

            modelBuilder.Entity<Todo>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Todos)
                .HasForeignKey(t => t.CategoryId);

            modelBuilder.Entity<Todo>()
                .HasOne(t => t.Plan)
                .WithMany(p => p.Todos)
                .HasForeignKey(t => t.PlanId);

        
        }

    
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=123456;Database=postgresqldb");
        }
    }
}
