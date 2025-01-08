namespace MyStore.Models
{
    public class Address : BaseEntity
    {
        public string StreetLine1 {  get; set; }
        public string? StreetLine2 { get; set; }
        public string City {  get; set; }
        public string County {  get; set; }
        public string Country {  get; set; }
        public string PostalCode {  get; set; }
        public string? AdditionalDetails {  get; set; }
        // Foreign Key for User
        public int UserId { get; set; }
        public User User { get; set; }

    }
}
