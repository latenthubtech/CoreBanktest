namespace CoreBankerApp.Models
{
    public record Industry
    {
        public int industryId { get; init; }
        public string name { get; init; }
        public ICollection<IndustryType> industryTypes { get; init; } = new List<IndustryType>();
    }
}
