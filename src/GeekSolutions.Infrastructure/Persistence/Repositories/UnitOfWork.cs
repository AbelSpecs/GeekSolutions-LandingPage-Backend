using System.Collections.Concurrent;
using GeekSolutions.Application.Interfaces.Persistence;
using GeekSolutions.Infrastructure.Persistence;

namespace GeekSolutions.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    public IContactsRepository Contact {  get; }
    private readonly ApplicationDbContext _context;
    private readonly ConcurrentDictionary<string, object> _repositories = new();

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IGenericRepository<T> Repository<T>() where T : class
    {
        var typeName = typeof(T).Name;

        return (IGenericRepository<T>)_repositories.GetOrAdd(typeName, _ =>
            new GenericRepository<T>(_context));
    }

    public async Task<int> SaveChanges(CancellationToken cancellationToken = default) => await _context.SaveChangesAsync(cancellationToken);

    public void Dispose() => _context.Dispose();
}