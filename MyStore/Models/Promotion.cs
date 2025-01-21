using System.ComponentModel.DataAnnotations;

namespace MyStore.Models
{
    public class Promotion:BaseEntity
    {
        [Required]
        public double Value {  get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

    }
}
