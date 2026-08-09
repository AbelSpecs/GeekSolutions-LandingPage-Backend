using System.Linq.Expressions;

namespace GeekSolutions.Application.Interfaces.Persistence;

public interface IUnitOfWork
{
    IGenericRepository<T> Repository<T>() where T : class;
    IContactsRepository Contact { get; }
    Task<int> SaveChanges(CancellationToken cancellationToken);
}