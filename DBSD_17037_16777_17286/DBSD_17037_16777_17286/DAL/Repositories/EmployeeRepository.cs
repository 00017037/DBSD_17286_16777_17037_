using AutoMapper;
using DBSD_17037_16777_17286.DAL.Infrastructure;
using DBSD_17037_16777_17286.DAL.Models;
using DBSD_17037_16777_17286.Models;
using Microsoft.EntityFrameworkCore;

namespace DBSD_17037_16777_17286.DAL.Repositories
{
    public class EmployeeRepository : IRepository<Employee>
    {
        private readonly MacroDbContext _context;

        public EmployeeRepository(MacroDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Employee> GetAll()
        {
            var empl = _context.Employees;
            
             
            
            var employees = _context.Employees
                                    .Include(e => e.Person) // Include Person navigation property
                                    .ToList(); // Materialize query to avoid lazy loading issues

            return employees;
        }

        public Employee GetById(int id)
        {
            var employee = _context.Employees
                        .Include(e => e.Person)
                        .SingleOrDefault(e => e.Id == id);
            return employee;
        }

        public int Insert(Employee entity)
        {
            var employee = _context.Employees.Add(entity).Entity;
            _context.SaveChanges();
            return employee.Id;
        }

        public void Update(Employee entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            Employee entity = _context.Employees.Find(id);
            _context.Employees.Remove(entity);
            _context.SaveChanges();
        }
    }
}
