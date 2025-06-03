namespace CoreBankerApi.Domain.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;

    [Table("TaskQueue")]
    public record TaskQueue: BaseModel
    {
        private TaskQueue(string referenceId, string taskType, string payload, int loanRequestId)
        {
            ReferenceId = referenceId;
            TaskType = taskType;
            Payload = payload;
            LoanRequestId = loanRequestId;
        }

        // Foreign Key
        public int LoanRequestId { get; set; }

        // Navigation Property
        public LoanRequest LoanRequest { get; set; }
        public string ReferenceId { get; private set; }
        public string TaskType { get; private set; } = string.Empty;
        public string Payload { get; private set; } = string.Empty;
        public string Status { get; private set; } = "Pending";
        public DateTime? ProcessedAt { get; private set; }

        public static TaskQueue NewInstance(string ReferenceId, string TaskType, String Payload, int LoanRequestId)
        {
            return new TaskQueue(ReferenceId, TaskType, Payload, LoanRequestId);
        }

        public void ChangeStatus(string status)
        {
            Status = status;
            ProcessedAt = DateTime.UtcNow;
        }
    }

}
