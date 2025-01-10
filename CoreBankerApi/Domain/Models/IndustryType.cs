using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CoreBankerApi.Domain.Models
{
    public record IndustryType: BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IndustryTypeId { get; init; }
        public string Name { get; init; }
        public string Label { get; init; }
        public int IndustryId { get; init; }
        public Industry Industry { get; init; }
    }
}
