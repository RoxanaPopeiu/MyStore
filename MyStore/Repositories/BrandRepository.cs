using Microsoft.EntityFrameworkCore;
using MyStore.Common;
using MyStore.Interfaces.Repositories;
using MyStore.Models;

namespace MyStore.Repositories
{
    public class BrandRepository(ApplicationDbContext context) : BaseRepository<Brand>(context), IBrandRepository
    {
        public async Task<bool> ExistsByIdOrNameAsync(int id, string name)
        {
            return await _dbSet.AnyAsync(b => b.Id == id || b.Name == name);
        }
        public async Task<bool> ExistsByNameAsync(string brandName)
        {
            return await _dbSet.AnyAsync(x => x.Name == brandName);

        }
    }
}
