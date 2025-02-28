using Microsoft.EntityFrameworkCore;
using MyStore.Common;
using MyStore.Interfaces.Repositories;
using MyStore.Models;

namespace MyStore.Repositories
{
    public class SizeRepository(ApplicationDbContext context) : BaseRepository<Size>(context), ISizeRepository
    {
        public async Task<List<Size>> GetByNameAsync(IEnumerable<string> names)
        {
            return await _dbSet.Where(s => names.Contains(s.Name)).ToListAsync();
        }
        public async Task<bool> CheckSizeExistence(string sizeName)
        {
            if (string.IsNullOrWhiteSpace(sizeName))
                return false;  

            return await _dbSet.AnyAsync(x => x.Name == sizeName);
        }
        public async Task<Size?> GetBySizeIdAsync(int sizeId)
        {
            return await _dbSet.FirstOrDefaultAsync(s => s.Id == sizeId);
        }
    }
}
