using System.Net;

namespace MyStore.Models
{
    public class User : BaseEntity
    {
        public string UserName { get; set; }
        public string Email {  get; set; }
        public string Password { get; set; }
        public string Role {  get; set; }
        // One-to-Many: One User can have multiple Addresses
        public ICollection<Address> Addresses { get; set; } = new List<Address>();

    }
}
