using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoApp.Core.Entities;

namespace TodoApp.Repositories.Configurations;

public class TodoCollaboratorConfiguration : IEntityTypeConfiguration<TodoCollaborator>
{
    public void Configure(EntityTypeBuilder<TodoCollaborator> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn();

        builder.Property(x => x.IsAccepted)
            .IsRequired()
            .HasDefaultValue(false);

     
        builder.HasOne(x => x.User)
            .WithMany(x => x.CollaborativeTodos)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Todo)
            .WithMany(x => x.Collaborators)
            .HasForeignKey(x => x.TodoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("TodoCollaborators");
    }
} 