
namespace BackgroundClient.Database
{
    using CoreBankerApi.Domain.Models;
    using Microsoft.EntityFrameworkCore;

    public class DatabaseContext : DbContext
    {
        public DbSet<TaskQueue> TaskQueue { get; set; }
        public DbSet<LoanRequest> LoanRequest { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer("Server=localhost;Database=CoreBanker;Trusted_Connection=True;Encrypt=False;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskQueue>()
                .HasOne(p => p.LoanRequest) // Product has one Category
                .WithMany(c => c.TaskQueues) // Category has many Products
                .HasForeignKey(p => p.LoanRequestId); // Foreign key in Product
        }
    }

    

}
