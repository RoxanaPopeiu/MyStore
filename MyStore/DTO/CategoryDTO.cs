using MyStore.Models;

namespace MyStore.DTO
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        // The name of the associated Brand
        public string BrandName { get; set; }

        // List of associated Sizes
        public List<SizeDto> Sizes { get; set; } = new List<SizeDto>();
    }
}
