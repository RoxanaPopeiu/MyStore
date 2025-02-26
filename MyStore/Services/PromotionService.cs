using Microsoft.EntityFrameworkCore;
using MyStore.DTO;
using MyStore.Interfaces.Services;
using MyStore.Mapping;
using MyStore.Models;

namespace MyStore.Services
{
    public class PromotionService(ApplicationDbContext context): IPromotionService
    {
        #region CRUD
        public async Task<PromotionDto> Create(PromotionDto promotionDto)
        {
            ValidatePromotion(promotionDto);
            if (await CheckPromotionExistence(promotionDto.StartDate, promotionDto.EndDate, promotionDto.Value))
                throw new PromotionAlreadyExistsException("The promotion already exists!");

            var promotion = promotionDto.ToPromotion();
            var result = await context.Promotions.AddAsync(promotion);
            await context.SaveChangesAsync();
            return result.Entity.ToPromotionDto();
        }

        public async Task<List<PromotionDto>> ReadAllPromotions()
        {
            var promotions = await context.Promotions.ToListAsync();
            return PromotionMapping.ToPromotionDtoList(promotions);
        }

        public async Task<PromotionDto> ReadOnePromotionById(int id)
        {
            var promotion = await context.Promotions.FindAsync(id);
            return promotion?.ToPromotionDto();
        }

        public async Task<PromotionDto> Update(int id, PromotionDto promotionDto)
        {
            ValidatePromotion(promotionDto);

            if (await CheckPromotionExistence(promotionDto.StartDate, promotionDto.EndDate, promotionDto.Value))
                throw new PromotionAlreadyExistsException("A promotion with the same details already exists!");

            var extPromotion = await context.Promotions.FindAsync(id);
            if (extPromotion == null)
                throw new PromotionNotFoundException("The promotion doesn't exist!");

            extPromotion.StartDate = promotionDto.StartDate;
            extPromotion.EndDate = promotionDto.EndDate;
            extPromotion.Value = promotionDto.Value;
            await context.SaveChangesAsync();
            return extPromotion.ToPromotionDto();
        }

        public async Task<bool> Delete(int id)
        {
            var extPromotion = await context.Promotions.FindAsync(id);
            if (extPromotion == null)
                return false;
            context.Promotions.Remove(extPromotion);
            await context.SaveChangesAsync();
            return true;
        }
        #endregion
        #region Exception Handling
        public class PromotionNotFoundException : Exception
        {
            public PromotionNotFoundException(string message) : base(message) { }
        }

        public class PromotionAlreadyExistsException : Exception
        {
            public PromotionAlreadyExistsException(string message) : base(message) { }
        }

        public class ValidationException : Exception
        {
            public ValidationException(string message) : base(message) { }
        }
        #endregion

        #region Private Methods
        private void ValidatePromotion(PromotionDto promotionDto)
        {
            if (promotionDto.StartDate >= promotionDto.EndDate)
                throw new ValidationException("Start date must be before end date.");
            if (promotionDto.EndDate < DateTime.UtcNow)
                throw new ValidationException("End date cannot be in the past.");
            if (promotionDto.Value < 0 || promotionDto.Value > 100)
                throw new ValidationException("Invalid discount value.");

            promotionDto.StartDate = DateTime.SpecifyKind(promotionDto.StartDate, DateTimeKind.Utc);
            promotionDto.EndDate = DateTime.SpecifyKind(promotionDto.EndDate, DateTimeKind.Utc);
        }

        private async Task<bool> CheckPromotionExistence(DateTime startDate, DateTime endDate, double value)
        {
            return await context.Promotions.AnyAsync(X =>
                X.StartDate.Date == startDate.Date &&
                X.EndDate.Date == endDate.Date &&
                X.Value == value);
        }
        #endregion

    }
}
