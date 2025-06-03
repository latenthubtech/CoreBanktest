namespace CoreBankerApp.Models
{
    public record IndustryType
    {
        public int industryTypeId { get; init; }
        public string name { get; init; }
        public string label { get; init; }
    }
}
