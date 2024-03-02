

using dataContext;
using interfaces;

namespace repositories;

public class UnitOfWork : IUnitOfWork
{
    private ITaskRepository? _taskRepo;
    public DataContext _context;

    public UnitOfWork(DataContext context)
    {
        _context = context;
    }

    public ITaskRepository TaskRepository
    {
        get
        {
            return _taskRepo = _taskRepo ?? new TaskRepository(_context);
        }
    }


    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}