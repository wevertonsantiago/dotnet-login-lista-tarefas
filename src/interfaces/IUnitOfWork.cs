namespace interfaces;

public interface IUnitOfWork
{
    ITaskRepository TaskRepository { get; }

    Task SaveChangesAsync();
}