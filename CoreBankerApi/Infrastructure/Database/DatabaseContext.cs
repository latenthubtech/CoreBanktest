using System.Reflection.Metadata;
using CoreBankerApi.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreBankerApi.Infrastructure.Database
{
    public class DatabaseContext: DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }  // Customers Entity
        public DbSet<Industry> Industries { get; set; }  // Industries Entity
        public DbSet<IndustryType> IndustryTypes { get; set; }  // IndustryTy[e Entity

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure one-to-many relationship
            //modelBuilder.Entity<Industry>()
            //    .HasMany(e => e.IndustryTypes)
            //    .WithOne(e => e.Industry)
            //    .IsRequired();

            // Configure many-to-many relationship
            //modelBuilder.Entity<Customer>()
            //    .HasMany(s => s.Industries)
            //    .WithMany(c => c.Customers)
            //    .UsingEntity(j => j.ToTable("CustomerIndustries"));

            // Seed data for Industry
            modelBuilder.Entity<Industry>().HasData(
                new Industry { IndustryId = 1, Name = "Manufacturing" },
                new Industry { IndustryId = 2, Name = "Education" },
                new Industry { IndustryId = 3, Name = "Telecom" }
            );

            // Seed data for Customer
            modelBuilder.Entity<Customer>().HasData(
                new Customer { CustomerId = 1, AccountNumber = "1234567890", IndustryId = 1 },
                new Customer { CustomerId = 2, AccountNumber = "2345678901", IndustryId = 2 },
                new Customer { CustomerId = 3, AccountNumber = "3456789012", IndustryId = 3 }
            );

            


            // Seed data for Industry Type
            modelBuilder.Entity<IndustryType>().HasData(
                new IndustryType { IndustryTypeId = 1, Name = "Invoice Number", Label = "Invoice Number", IndustryId = 1 },
                new IndustryType { IndustryTypeId = 2, Name = "Quantity", Label = "Quantity", IndustryId = 1 },
                new IndustryType { IndustryTypeId = 3, Name = "Delivery Address", Label = "Delivery Address", IndustryId = 1 },
                new IndustryType { IndustryTypeId = 4, Name = "Invoice Number", Label = "Invoice Number", IndustryId = 2 },
                new IndustryType { IndustryTypeId = 5, Name = "Level", Label = "Level", IndustryId = 2 },
                new IndustryType { IndustryTypeId = 6, Name = "Course", Label = "Course", IndustryId = 2 },
                new IndustryType { IndustryTypeId = 7, Name = "GSM Number", Label = "GSM Number", IndustryId = 3 },
                new IndustryType { IndustryTypeId = 8, Name = "Level", Label = "Network", IndustryId = 3 },
                new IndustryType { IndustryTypeId = 9, Name = "Course", Label = "Residential Address", IndustryId = 3 }
            );

            // Seed data for the junction table (CustomerIndustries)
            //modelBuilder.Entity<Customer>()
            //    .HasMany(i => i.Industries)
            //    .WithMany(c => c.Customers)
            //    .UsingEntity(j => j.HasData(
            //        new { CustomersCustomerId = 1, IndustriesIndustryId = 1 }, 
            //        new { CustomersCustomerId = 2, IndustriesIndustryId = 2 }, 
            //        new { CustomersCustomerId = 3, IndustriesIndustryId = 3 }  
            //    ));
        }
    }
}
