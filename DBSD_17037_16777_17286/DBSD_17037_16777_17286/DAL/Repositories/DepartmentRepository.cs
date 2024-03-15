﻿namespace DBSD_17037_16777_17286.DAL.Repositories
{

    using global::DBSD_17037_16777_17286.DAL.Infrastructure;
    using global::DBSD_17037_16777_17286.DAL.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    namespace DBSD_17037_16777_17286.DAL.Repositories
    {
        public class DepartmentRepository : IRepository<Department>
        {
            private readonly MacroDbContext _context;

            public DepartmentRepository(MacroDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<Department>> GetAll()
            {
                return await _context.Departments
                    .Include(d => d.Manager)
                    .ToListAsync();
            }

            public async Task<Department> GetById(int id)
            {
                return await _context.Departments
                    .Include(d => d.Manager)
                    .FirstOrDefaultAsync(d => d.Id == id);
            }

            public int Insert(Department entity)
            {
                _context.Departments.Add(entity);
                _context.SaveChanges();
                return entity.Id;
            }

            public void Update(Department entity)
            {
                _context.Entry(entity).State = EntityState.Modified;
                _context.SaveChanges();
            }

            public void Delete(int id)
            {
                var entity = _context.Departments.Find(id);
                if (entity != null)
                {
                    _context.Departments.Remove(entity);
                    _context.SaveChanges();
                }
            }
        }
    }

}
