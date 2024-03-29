using AutoMapper;
using DBSD_17037_16777_17286.DAL.Infrastructure;
using DBSD_17037_16777_17286.DAL.Models;
using DBSD_17037_16777_17286.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

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
            var employees = new List<Employee>();

            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "GetAllEmployees";
                    command.CommandType = CommandType.StoredProcedure;

                    await _context.Database.OpenConnectionAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
                                var employee = MapEmployeeFromReader(reader);
                                employees.Add(employee);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine(ex.Message);
            }

            return employees;
        }

        public async Task<Employee> GetById(int id)
        {
            Employee employee = null;

            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "GetEmployeeById";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@Id", id));

                    await _context.Database.OpenConnectionAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            await reader.ReadAsync();
                            employee = MapEmployeeFromReader(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine(ex.Message);
            }

            return employee;
        }

        public int Insert(Employee entity)
        {
            var hireDateParam = new SqlParameter("@HireDate", entity.HireDate);
            var hourlyRateParam = new SqlParameter("@HourlyRate", entity.HourlyRate);
            var isMarriedParam = new SqlParameter("@isMarried", entity.IsMarried);
            var photoParam = new SqlParameter("@Photo", SqlDbType.VarBinary, -1);
            photoParam.Value = (object)entity.Photo ?? (object)Array.Empty<byte>();
            var departmentIdParam = new SqlParameter("@DepartmentId", entity.DepartmentId);
            var managerIdParam = new SqlParameter("@ManagerId", entity.ManagerId);
            var personIdParam = new SqlParameter("@PersonId", entity.PersonId);

            var idParam = new SqlParameter("@Id", SqlDbType.Int) { Direction = ParameterDirection.Output };

            var id = _context.Database.ExecuteSqlRaw("InsertEmployee @HireDate, @HourlyRate, @isMarried, @Photo, @DepartmentId, @ManagerId, @PersonId",
                hireDateParam, hourlyRateParam, isMarriedParam, photoParam, departmentIdParam, managerIdParam, personIdParam);

            return (int)id;
        }

        public void Update(Employee entity)
        {
            var idParam = new SqlParameter("@Id", entity.Id);
            var hireDateParam = new SqlParameter("@HireDate", entity.HireDate);
            var hourlyRateParam = new SqlParameter("@HourlyRate", entity.HourlyRate);
            var isMarriedParam = new SqlParameter("@isMarried", entity.IsMarried);
            var photoParam = new SqlParameter("@Photo", SqlDbType.VarBinary, -1);
            photoParam.Value = (object)entity.Photo ?? (object)Array.Empty<byte>();

            var departmentIdParam = new SqlParameter("@DepartmentId", entity.DepartmentId);
            var managerIdParam = new SqlParameter("@ManagerId", entity.ManagerId);
            var personIdParam = new SqlParameter("@PersonId", entity.PersonId);

            _context.Database.ExecuteSqlRaw("UpdateEmployee @Id, @HireDate, @HourlyRate, @isMarried, @Photo, @DepartmentId, @ManagerId, @PersonId",
                idParam, hireDateParam, hourlyRateParam, isMarriedParam, photoParam, departmentIdParam, managerIdParam, personIdParam);
        }

        public void Delete(int id)
        {
            var idParam = new SqlParameter("@Id", id);
            _context.Database.ExecuteSqlRaw("DeleteEmployee @Id", idParam);
        }
        public async Task<IEnumerable<Employee>> Filter(string firstName,string lastName , string departmentName, DateTime? hireDate, string sortField, bool sortDesc, int page, int pageSize=10)
        {

            var employees = new List<Employee>();
   
            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "udpFilterEmployees";
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@FirstName", firstName ?? (object)DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@LastName", lastName ?? (object)DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@DepartmentName", departmentName ?? (object)DBNull.Value));

                    command.Parameters.Add(new SqlParameter("@HireDate", hireDate ?? (object)DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@SortField", sortField));
                    command.Parameters.Add(new SqlParameter("@SortDesc", sortDesc));
                    command.Parameters.Add(new SqlParameter("@Page", page));
                    command.Parameters.Add(new SqlParameter("@PageSize", pageSize));

                    await _context.Database.OpenConnectionAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
                                var employee = MapEmployeeFromReader(reader);
                                employees.Add(employee);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine(ex.Message);
            }

            return employees;
        }


       
        private Employee MapEmployeeFromReader(DbDataReader reader)
        {
            var employee = new Employee
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                HireDate = reader.GetDateTime(reader.GetOrdinal("HireDate")),
                HourlyRate = reader.GetDecimal(reader.GetOrdinal("HourlyRate")),
                IsMarried = reader.GetBoolean(reader.GetOrdinal("isMarried")),
                Photo = reader.IsDBNull(reader.GetOrdinal("Photo")) ? null : (byte[])reader["Photo"],
                DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                Department = new Department { Name = reader.GetString(reader.GetOrdinal("DepartmentName")) },
                PersonId = reader.GetInt32(reader.GetOrdinal("PersonId")),
                Person = new Person
                {
                    ContactDetails = reader.IsDBNull(reader.GetOrdinal("ContactDetails")) ? "" : reader.GetString(reader.GetOrdinal("ContactDetails")),
                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                    LastName = reader.GetString(reader.GetOrdinal("LastName"))
                }
            };

            // Check if the manager details are available
            if (!reader.IsDBNull(reader.GetOrdinal("ManagerId")))
            {
                employee.ManagerId = reader.GetInt32(reader.GetOrdinal("ManagerId"));
                employee.Manager = new Employee
                {
                    Id = reader.GetInt32(reader.GetOrdinal("ManagerPersonId")),
                    PersonId = reader.GetInt32(reader.GetOrdinal("ManagerPersonId")),
                    Person = new Person
                    {
                        FirstName = reader.GetString(reader.GetOrdinal("ManagerFirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("ManagerLastName"))
                    }
                };
            }

            return employee;
        }
    }
}
