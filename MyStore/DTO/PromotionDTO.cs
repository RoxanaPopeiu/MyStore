using System.ComponentModel.DataAnnotations;

namespace MyStore.DTO
{
    public class PromotionDto
    {
        public int? Id { get; set; }
        [Required]
        [Range(0, 100, ErrorMessage = "Value must be between 0 and 100.")]
        public double Value { get; set; }
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
        // List of associated products
        public List<ProductDto> Products { get; set; } = new List<ProductDto>();
    }
}
