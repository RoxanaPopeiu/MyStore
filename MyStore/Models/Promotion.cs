namespace MyStore.Models
{
    public class Promotion:BaseEntity
    {
        public double Value {  get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
