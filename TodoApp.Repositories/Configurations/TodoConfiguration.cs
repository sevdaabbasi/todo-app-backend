using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoApp.Core.Entities;

namespace TodoApp.Repositories.Configurations;

public class TodoConfiguration : IEntityTypeConfiguration<Todo>
{
    public void Configure(EntityTypeBuilder<Todo> builder)
    {
        // Primary Key
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.Id).UseIdentityColumn();
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(1000);
        builder.Property(x => x.DueDate)
            .IsRequired();
        builder.Property(x => x.Priority)
            .IsRequired();
        builder.Property(x => x.IsCompleted)
            .IsRequired()
            .HasDefaultValue(false);

        // Relationships
        builder.HasOne(x => x.Category)
            .WithMany(x => x.Todos)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.User)
            .WithMany(x => x.Todos)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Plan)
            .WithMany(x => x.Todos)
            .HasForeignKey(x => x.PlanId)
            .OnDelete(DeleteBehavior.SetNull);

        // Table Mapping (Optional)
        builder.ToTable("Todos");
    }
}