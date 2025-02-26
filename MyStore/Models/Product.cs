namespace MyStore.Models
{
    public class Product:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price {  get; set; }
        public int StockQuantity { get; set; }
        public bool IsAvailable { get; set; }

        // Foreign Key for Category
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        // Foreign Key for Brand
        public int BrandId { get; set; }
        public Brand Brand { get; set; }

        // Foreign Key for Promotion
        public int? PromotionId { get; set; } // Optional
        public Promotion Promotion { get; set; }

        // Relația Many-to-Many cu Size prin ProductSize
        public ICollection<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();


    }
}
