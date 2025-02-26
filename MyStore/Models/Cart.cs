namespace MyStore.Models
{
    public class Cart: BaseEntity
    {
        // Foreign Key for User
        public int UserId { get; set; }
        public User User { get; set; }
        public double TotalPrice {  get; set; }
        //navigation property 
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
