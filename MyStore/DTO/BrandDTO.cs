using MyStore.Models;

namespace MyStore.DTO
{
    public class BrandDto
    {
        public int Id { get; set; }
           
        public string Name { get; set; }
        public string Description { get; set; }
        // One-to-Many: One Brand can have multiple Categories
        public ICollection<CategoryDto> Categories { get; set; } = new List<CategoryDto>();

    }
}
