using Microsoft.EntityFrameworkCore;
using MyStore.Common;
using MyStore.Interfaces.Repositories;
using MyStore.Models;

namespace MyStore.Repositories
{
    public class CategoryRepository(ApplicationDbContext context) : BaseRepository<Category>(context), ICategoryRepository
    {
        public async Task<bool> ExistsByIdOrNameAsync(int id, string name)
        {
            return await _dbSet.AnyAsync(c => c.Id == id || c.Name == name);
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false; 

            return await _dbSet.AnyAsync(c => c.Name == name);
        }
    }
}
