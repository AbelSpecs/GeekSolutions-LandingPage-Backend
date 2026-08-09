using System.Linq.Expressions;
using GeekSolutions.Application.Interfaces.Persistence;
using GeekSolutions.Infrastructure.Persistence;
using GeekSolutions.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GeekSolutions.Infrastructure.Persistence.Repositories;

public class ContactRepository : IContactsRepository
{
    private readonly ApplicationDbContext _context;

    public ContactRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public bool Delete(string id)
    {
        if (!int.TryParse(id, out var intId)) return false;
        var entity = _context.Set<Contact>().Find(intId);
        if (entity == null) return false;
        _context.Set<Contact>().Remove(entity);
        return _context.SaveChanges() > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        if (!int.TryParse(id, out var intId)) return false;
        var entity = await _context.Set<Contact>().FindAsync(intId);
        if (entity == null) return false;
        _context.Set<Contact>().Remove(entity);
        var rows = await _context.SaveChangesAsync();
        return rows > 0;
    }

    public IEnumerable<Contact> GetAll()
    {
        return _context.Set<Contact>().AsNoTracking();
    }

    public async Task<IEnumerable<Contact>> GetAllAsync()
    {
        return await _context.Set<Contact>().AsNoTracking().ToListAsync();
    }

    public IEnumerable<Contact> GetAllWithPagination(int pageNumber, int pageSize)
    {
        return _context.Set<Contact>().AsNoTracking().Skip((pageNumber - 1) * pageSize).Take(pageSize);
    }

    public async Task<IEnumerable<Contact>> GetAllWithPaginationAsync(int pageNumber, int pageSize)
    {
        return await _context.Set<Contact>().AsNoTracking().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
    }

    public Contact Get(string id)
    {
        if (!int.TryParse(id, out var intId)) return null;
        return _context.Set<Contact>().Find(intId);
    }

    public async Task<Contact> GetAsync(string id)
    {
        if (!int.TryParse(id, out var intId)) return null;

        return await _context.Contact
        .AsNoTracking()
        .FirstAsync(c => c.Id == intId);
    }

    public int Count()
    {
        return _context.Set<Contact>().Count();
    }

    public async Task<int> CountAsync()
    {
        return await _context.Set<Contact>().CountAsync();
    }

    public bool Insert(Contact entity)
    {
        _context.Set<Contact>().Add(entity);
        return _context.SaveChanges() > 0;
    }

    public async Task<bool> InsertAsync(Contact entity)
    {
        await _context.Set<Contact>().AddAsync(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public bool Update(Contact entity)
    {
        _context.Set<Contact>().Update(entity);
        return _context.SaveChanges() > 0;
    }

    public async Task<bool> UpdateAsync(Contact entity)
    {
        _context.Set<Contact>().Update(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<Contact> InsertContactAsync(Contact contact)
    {
        await _context.Contact.AddAsync(contact);
        await _context.SaveChangesAsync();
        return contact;
    }

    public async Task<Contact> GetContactsByIdAsync(string id)
    {
        if (!int.TryParse(id, out var intId)) return null;
        return await _context.Contact
        .AsNoTracking()
        .Where(c => c.Id == intId)
        .FirstOrDefaultAsync();
    }

   

    
}