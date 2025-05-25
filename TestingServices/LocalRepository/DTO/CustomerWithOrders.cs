namespace LocalRepository.DTO
{
    public class CustomerWithOrders
    {
        public string CustomerID { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public List<OrderWithDetails> Orders { get; set; } = [];
    }
}