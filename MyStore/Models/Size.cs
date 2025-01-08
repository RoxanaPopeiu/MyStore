namespace MyStore.Models
{
    public class Size:BaseEntity
    {
        public string Name {  get; set; }
        public string Description {  get; set; }
        // Foreign Key for Category
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        // Relația Many-to-Many cu Product prin ProductSize
        public ICollection<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();
    }
}
