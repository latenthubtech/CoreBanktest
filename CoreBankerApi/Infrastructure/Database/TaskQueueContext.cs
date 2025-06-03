namespace CoreBankerApi.Infrastructure.Database
{
    using CoreBankerApi.Domain.Models;
    using Microsoft.EntityFrameworkCore;

    public class TaskQueueContext : DbContext
    {
        public DbSet<TaskQueue> TaskQueue { get; set; }
        public DbSet<LoanRequest> LoanRequest { get; set; }

        public TaskQueueContext(DbContextOptions<TaskQueueContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskQueue>()
                .HasOne(p => p.LoanRequest) // Product has one Category
                .WithMany(c => c.TaskQueues) // Category has many Products
                .HasForeignKey(p => p.LoanRequestId); // Foreign key in Product

            modelBuilder.Entity<TaskQueue>()
                .Property(t => t.Status)
                .HasDefaultValue("Pending");
        }
    }

}
