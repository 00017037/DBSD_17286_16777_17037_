
using DBSD_17037_16777_17286.DAL.Models;
using Microsoft.EntityFrameworkCore;
using DBSD_17037_16777_17286.Models;

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
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Employee>()
                        .Property(e => e.HourlyRate)
                        .HasPrecision(10, 2);

            modelBuilder.Entity<Transaction>()
                        .Property(t => t.Total)
                        .HasPrecision(12, 2);

            modelBuilder.Entity<Employee>()
                   .HasOne(e => e.Manager)
                   .WithMany()
                   .HasForeignKey(e => e.ManagerId)
                   .IsRequired(false);

            modelBuilder.Entity<Employee>()
               .HasIndex(e => e.ManagerId)
               .IsUnique(false); // Allow multiple employees to have null ManagerId

            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.Id) // Assuming Id is the primary key
                .HasFilter("[ManagerId] IS NOT NULL"); // Allow only one manager per employee


            modelBuilder.Entity<Department>()
               .HasOne(d => d.Manager)
               .WithOne()
               .HasForeignKey<Department>(d => d.ManagerId)
               .IsRequired(false);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany()
                .HasForeignKey(e => e.DepartmentId)
                .IsRequired(true);
            //modelBuilder.Entity<Transaction>()
            //    .HasOne(t => t.Employee)
            //    .WithMany()  // Assuming Employee has a collection of transactions
            //    .HasForeignKey(t => t.EmployeeId)
            //    .OnDelete(DeleteBehavior.Restrict);  // Change the delete behavior to Restrict





        }


        public DbSet<DBSD_17037_16777_17286.Models.EmployeeViewModel>? EmployeeViewModel { get; set; }


        }
    
}
