using System.ComponentModel.DataAnnotations.Schema;

namespace CoreBankerApi.Domain.Models
{
    [Table("LoanRequest")]
    public record LoanRequest: BaseModel
    {
        public string ReferenceId { get; set; }
        public string Status { get; set; } = "Pending";
        public ICollection<TaskQueue> TaskQueues { get; set; }
    }
}
