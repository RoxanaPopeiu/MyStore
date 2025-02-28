using Microsoft.EntityFrameworkCore;
using MyStore.DTO;
using MyStore.Interfaces.Repositories;
using MyStore.Interfaces.Services;
using MyStore.Mapping;
using MyStore.Models;

namespace MyStore.Services
{
    public class PromotionService(IPromotionRepository promotionRepository) : IPromotionService
    {
        #region CRUD
        public async Task<PromotionDto> Create(PromotionDto promotionDto)
        {
            ValidatePromotion(promotionDto);
            if (await promotionRepository.CheckPromotionExistence(promotionDto.StartDate, promotionDto.EndDate, promotionDto.Value))
                throw new PromotionAlreadyExistsException("The promotion already exists!");

            var promotion = promotionDto.ToPromotion();
            var result = await promotionRepository.AddAsync(promotion);
            return result.ToPromotionDto();
        }

        public async Task<List<PromotionDto>> ReadAllPromotions()
        {
            var promotions = await promotionRepository.GetAllAsync();
            return promotions.ToPromotionDtoList();
        }

        public async Task<PromotionDto?> ReadOnePromotionById(int id)
        {
            var promotion = await promotionRepository.GetByIdAsync(id);
            return promotion?.ToPromotionDto();
        }

        public async Task<PromotionDto> Update(int id, PromotionDto promotionDto)
        {
            ValidatePromotion(promotionDto);

            if (await promotionRepository.CheckPromotionExistence(promotionDto.StartDate, promotionDto.EndDate, promotionDto.Value))
                throw new PromotionAlreadyExistsException("A promotion with the same details already exists!");

            var extPromotion = await promotionRepository.GetByIdAsync(id);
            if (extPromotion == null)
                throw new PromotionNotFoundException("The promotion doesn't exist!");

            extPromotion.StartDate = promotionDto.StartDate;
            extPromotion.EndDate = promotionDto.EndDate;
            extPromotion.Value = promotionDto.Value;
            await promotionRepository.UpdateAsync(extPromotion);
            return extPromotion.ToPromotionDto();
        }

        public async Task<bool> Delete(int id)
        {
            var extPromotion = await promotionRepository.GetByIdAsync(id);
            if (extPromotion == null)
                return false;
            await promotionRepository.DeleteAsync(extPromotion);
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
        #endregion

    }
}
