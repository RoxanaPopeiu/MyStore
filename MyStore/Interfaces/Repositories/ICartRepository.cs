using MyStore.Common;
using MyStore.DTO;
using MyStore.Models;

namespace MyStore.Interfaces.Repositories
{
    public interface ICartRepository : IBaseRepository<Cart>
    {
        Task<CartDto> GetCartWithDetailsAsync(int userId);
        Task<Cart?> GetCartByUserIdAsync(int userId);

    }
}
