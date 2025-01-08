namespace MyStore.Models
{
    public class Brand:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        // One-to-Many: One Brand can have multiple Categories
        public ICollection<Category> Categories { get; set; } = new List<Category>();

    }
}
