namespace TodoApp.Core.Interfaces;

public interface IUnitOfWork : IDisposable
{
    Task CommitAsync();
    Task RollbackAsync();
}