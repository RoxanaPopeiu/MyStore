using MyStore.Models;

namespace MyStore.DTO
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public Cart? Cart { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; } // Navigation Property
        // Foreign Key for ProductSize
        public int? ProductSizeId { get; set; }
        public ProductSize? ProductSize { get; set; }
        public int Quantity { get; set; }
        public double PriceAtTimeOfAddition { get; set; }
    }
}
