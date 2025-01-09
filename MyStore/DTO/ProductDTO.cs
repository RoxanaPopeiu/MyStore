using MyStore.Models;

namespace MyStore.DTO
{
    public class ProductDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        // Associated Category
        public string  CategoryName { get; set; }
        // Associated Brand
        public string BrandName { get; set; }
        // Optional Promotion
        public string PromotionName{ get; set; }
        // List of associated Sizes
        public ICollection<SizeDto> Sizes { get; set; } = new List<SizeDto>();
    }
}
