using MyStore.DTO;
using MyStore.Models;

namespace MyStore.Mapping
{
    public static class PromotionMapping
    {
        // Map Promotion to PromotionDto
        public static PromotionDto ToPromotionDto(this Promotion promotion)
        {
            return new PromotionDto
            {
                Id = promotion.Id,
                Value = promotion.Value,
                StartDate = promotion.StartDate,
                EndDate = promotion.EndDate
            };
        }

        // Map a list of Promotions to a list of PromotionDtos
        public static List<PromotionDto> ToPromotionDtoList(this IEnumerable<Promotion> promotions)
        {
            return promotions.Select(promotion => promotion.ToPromotionDto()).ToList();
        }
    }
}
