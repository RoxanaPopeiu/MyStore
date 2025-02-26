using MyStore.DTO;
using MyStore.Mapping;
using static MyStore.Services.PromotionService;

namespace MyStore.Interfaces.Services
{
    public interface IPromotionService
    {
        public  Task<PromotionDto> Create(PromotionDto promotionDto);
        public Task<List<PromotionDto>> ReadAllPromotions();
        public Task<PromotionDto> ReadOnePromotionById(int id);
        public Task<PromotionDto> Update(int id, PromotionDto promotionDto);
        public Task<bool> Delete(int id);
        
    }
}
