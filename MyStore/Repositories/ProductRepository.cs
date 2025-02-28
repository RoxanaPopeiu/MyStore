using Microsoft.EntityFrameworkCore;
using MyStore.Common;
using MyStore.Interfaces.Repositories;
using MyStore.Models;

namespace MyStore.Repositories
{
    public class ProductRepository(ApplicationDbContext context) : BaseRepository<Product>(context), IProductRepository
    {
        public async Task<IEnumerable<Product>> GetProductsByCategory(int categoryId)
        {
            return await _dbSet.Where(p => p.CategoryId == categoryId).ToListAsync();

        }
        public async Task<Product?> GetByNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.Name == name);
        }
        public async Task<List<Product>> GetAllWithDetailsAsync()
        {
            return await _dbSet
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.ProductSizes)
                    .ThenInclude(ps => ps.Size)
                .ToListAsync();
        }
        public async Task<Product?> GetByIdWithDetailsAsync(int id)
        {
            return await _dbSet
                   .Include(p => p.Brand)
                   .Include(p => p.Category)
                   .Include(p => p.Promotion)
                   .Include(p => p.ProductSizes)
                      .ThenInclude(ps => ps.Size)
                   .SingleOrDefaultAsync(p => p.Id == id);
        }
        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _dbSet.AnyAsync(p=>p.Name == name);
        }

    }
}
