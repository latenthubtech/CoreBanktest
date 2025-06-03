namespace CoreBankerApp.Models
{
    public record Customer
    {
        public int customerId { get; init; }
        public string accountNumber { get; init; }
        public int industryId { get; init; }
        public Industry industry { get; init; }
    }
}
