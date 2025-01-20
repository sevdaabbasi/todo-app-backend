namespace TodoApp.Core.Exceptions;

public class NotFoundException : BaseException
{
    public NotFoundException(string name, object key)
        : base($"Entity \"{name}\" ({key}) was not found.")
    {
    }
} 