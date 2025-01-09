namespace MyStore.DTO
{
    public class AddressDto
    {
        public int? Id { get; set; }
        public string StreetLine1 { get; set; }
        public string? StreetLine2 { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string AdditionalDetails { get; set; }
    }
}
