
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ezyGo.EntityFrameworkCore.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly DbContext _db;
    private readonly DbSet<T> _table;

    public GenericRepository(DbContext db)
    {
        _db = db;
        _table = _db.Set<T>();
    }
    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _table.FindAsync(id);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _table.ToListAsync();
    }

    public virtual async Task<T> AddAsync(T entity)
   {
        _table.Add(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    public virtual async Task UpdateAsync(T entity)
    {
        _table.Update(entity);
        await _db.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(T entity)
    {
        _table.Remove(entity);
        await _db.SaveChangesAsync();
    }

    public virtual async Task<bool> IsExistsAsync(int id)
    {
        return await _table.FindAsync(id) != null;
    }

    public virtual IQueryable<T> GetQueryable()
    {
        return _table.AsQueryable();
    }
}
