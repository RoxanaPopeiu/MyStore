using MyStore.Models;

namespace MyStore.DTO
{
    public class CartDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public double TotalPrice { get; set; }
        //navigation property 
        public ICollection<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();
    }
}
