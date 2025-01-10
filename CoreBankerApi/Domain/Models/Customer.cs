using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace CoreBankerApi.Domain.Models
{
    public record Customer: BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; init; }
        public string AccountNumber { get; init; }
        public int IndustryId { get; init; }
        public Industry Industry { get; init; }
    }
}
