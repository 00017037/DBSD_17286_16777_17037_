using DBSD_17037_16777_17286.DAL.Infrastructure;
using DBSD_17037_16777_17286.DAL.Models;
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
            return _context.Employees.ToList();
        }

        public Employee GetById(int id)
        {
            return _context.Employees.Find(id);
        }

        public void Insert(Employee entity)
        {
            _context.Employees.Add(entity);
            _context.SaveChanges();
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
