using Microsoft.EntityFrameworkCore;
using MyStore.Common;
using MyStore.Interfaces.Repositories;
using MyStore.Models;

namespace MyStore.Repositories
{
    public class PromotionRepository(ApplicationDbContext context) : BaseRepository<Promotion>(context), IPromotionRepository
    {
        public async Task<bool> CheckPromotionExistence(DateTime startDate, DateTime endDate, double value)
        {
            return await context.Promotions.AnyAsync(X =>
                X.StartDate.Date == startDate.Date &&
                X.EndDate.Date == endDate.Date &&
                X.Value == value);
        }

    }
}
