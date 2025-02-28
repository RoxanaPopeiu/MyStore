using MyStore.Common;
using MyStore.Models;

namespace MyStore.Interfaces.Repositories
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsByCategory(int categoryId);
        Task<Product?> GetByNameAsync(string name);
        Task<List<Product>> GetAllWithDetailsAsync();
        Task<Product?> GetByIdWithDetailsAsync(int id);
        Task<bool> ExistsByNameAsync(string name);
    }
}
