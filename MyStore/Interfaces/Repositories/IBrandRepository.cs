using MyStore.Common;
using MyStore.Models;

namespace MyStore.Interfaces.Repositories
{
    public interface IBrandRepository : IBaseRepository<Brand>
    {
        public Task<bool> ExistsByIdOrNameAsync(int id, string name);
        public Task<bool> ExistsByNameAsync(string brandName);
    }
}
