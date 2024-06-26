﻿using DBSD_17037_16777_17286.DAL.Infrastructure;
using DBSD_17037_16777_17286.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DBSD_17037_16777_17286.DAL.Repositories
{
    public class PersonRepository : IRepository<Person>
    {
        private readonly MacroDbContext _context;

        public PersonRepository(MacroDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Person>> GetAll()
        {
            return await _context.Persons.ToListAsync();
        }

        public async Task<Person> GetById(int id)
        {
            return await _context.Persons.FirstOrDefaultAsync(p => p.Id == id);
        }

        public int Insert(Person entity)
        {
            _context.Persons.Add(entity);
            _context.SaveChanges();
            return entity.Id;
        }

        public void Update(Person entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = _context.Persons.Find(id);
            if (entity != null)
            {
                _context.Persons.Remove(entity);
                _context.SaveChanges();
            }
        }

        public string ExportToXml(int Id, string? FirstName, string? LastName, DateTime? HireDate, bool IsMarried, string? ManagerFirstName, string? ManagerLastName)
        {
            throw new NotImplementedException();
        }

        
        public Task<IEnumerable<Person>>? Filter(string firstName, string lastName, string departmentName, DateTime? hireDate, string sortField, bool sortDesc, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public string ExportToJson(int Id, string? FirstName, string? LastName, DateTime? HireDate, bool IsMarried, string? ManagerFirstName, string? ManagerLastName)
        {
            throw new NotImplementedException();
        }
    }
}
