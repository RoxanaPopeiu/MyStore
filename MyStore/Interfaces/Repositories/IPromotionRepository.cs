using MyStore.Common;
using MyStore.Models;

namespace MyStore.Interfaces.Repositories
{
    public interface IPromotionRepository : IBaseRepository<Promotion>
    {
        Task<bool> CheckPromotionExistence(DateTime startDate, DateTime endDate, double value);
    }
}
