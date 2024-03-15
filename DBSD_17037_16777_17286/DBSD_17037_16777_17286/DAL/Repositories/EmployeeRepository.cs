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

        public async Task<IEnumerable<Employee>> GetAll()
        {
            var empl = _context.Employees.ToList();


            Console.WriteLine(empl);
            var employees =  await _context.Employees
                                    .Include(e=>e.Person)
                                    .Include(e=>e.Department)
                                    .Include(e=>e.Manager)
                                    .ToListAsync(); // Materialize query to avoid lazy loading issues

            return employees;
        }

        public async Task<Employee> GetById(int id)
        {
            var employee = await  _context.Employees
                        .Include(e => e.Person)
                         .Include(e => e.Department)
                         .Include(e => e.Manager.Person)
                        .SingleOrDefaultAsync(e => e.Id == id);
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
            if (entity != null)
            {
                // Find employees referencing this employee as manager
                var employeesWithThisManager = _context.Employees.Where(e => e.ManagerId == id);
                foreach (var employee in employeesWithThisManager)
                {
                    // Set ManagerId to null for each referencing employee
                    employee.ManagerId = null;
                }

                _context.Employees.Remove(entity);
                _context.SaveChanges();
            }
        }
    }
}
