namespace SecureCodingWebshop.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserEmail { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
    }
}