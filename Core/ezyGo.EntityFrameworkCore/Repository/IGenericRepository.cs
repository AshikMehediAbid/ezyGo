namespace ezyGo.EntityFrameworkCore.Repository;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(int id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity> AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
    Task<bool> IsExistsAsync(int id);
    IQueryable<TEntity> GetQueryable();
}
