using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CoreBankerApi.Domain.Models
{
    public record Industry: BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IndustryId { get; init; }
        public string Name { get; init; }
        public ICollection<IndustryType> IndustryTypes { get; init; } = new List<IndustryType>();
        public ICollection<Customer> Customers { get; init; } = new List<Customer>();
    }
}
