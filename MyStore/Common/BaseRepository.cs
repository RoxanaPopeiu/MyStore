
using Microsoft.EntityFrameworkCore;

namespace MyStore.Common
{
    public class BaseRepository<T>(ApplicationDbContext context) : IBaseRepository<T>
         where T : class
    {
        protected readonly DbSet<T> _dbSet = context.Set<T>();

        public async Task<IEnumerable<T>> GetAllAsync()
        {
           return await _dbSet.ToListAsync();
        }
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }
        public async Task<T> AddAsync(T entity)
        {
            var result = await _dbSet.AddAsync(entity);
            await context.SaveChangesAsync();
            return result.Entity;
        }
        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await context.SaveChangesAsync();
        }
        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await context.SaveChangesAsync();
        }
    }
}
