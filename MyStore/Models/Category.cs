namespace MyStore.Models
{
    public class Category:BaseEntity
    {
        public string Name {  get; set; }
        public string Description {  get; set; }
        // Foreign Key for Brand
        public int BrandId { get; set; }
        public Brand Brand { get; set; }

        // One-to-Many: One Category can have multiple Sizes
        public ICollection<Size> Sizes { get; set; } = new List<Size>();
    }
}
