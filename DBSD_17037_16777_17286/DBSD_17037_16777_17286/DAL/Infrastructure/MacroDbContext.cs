
using DBSD_17037_16777_17286.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DBSD_17037_16777_17286.DAL.Infrastructure
{
    public class MacroDbContext:DbContext
    {

        public MacroDbContext(DbContextOptions<MacroDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Person> Persons { get; set; }
         
        public DbSet<Transaction> Transactions { get; set; }
         
        public DbSet<Customer> Customers { get; set; }  

        public DbSet<Department> Departments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                        .Property(e => e.HourlyRate)
                        .HasPrecision(10, 2);

            modelBuilder.Entity<Transaction>()
                        .Property(t => t.Total)
                        .HasPrecision(12, 2);


            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Employee)
                .WithMany()  // Assuming Employee has a collection of transactions
                .HasForeignKey(t => t.EmployeeID)
                .OnDelete(DeleteBehavior.Restrict);  // Change the delete behavior to Restrict




            base.OnModelCreating(modelBuilder);
        }


        }
    
}
