using MyStore.Common;
using MyStore.Models;

namespace MyStore.Interfaces.Repositories
{
    public interface ISizeRepository : IBaseRepository<Size>
    {
        Task<List<Size>> GetByNameAsync(IEnumerable<string> names);
        Task<bool> CheckSizeExistence(string sizeName);
        Task<Size?> GetBySizeIdAsync(int sizeId);
    }
}
