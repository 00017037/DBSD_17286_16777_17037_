using AutoMapper;
using DBSD_17037_16777_17286.DAL.Infrastructure;
using DBSD_17037_16777_17286.DAL.Models;
using DBSD_17037_16777_17286.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Dapper;

namespace DBSD_17037_16777_17286.DAL.Repositories
{
    public class EmployeeRepository : IRepository<Employee>
    {
        private readonly MacroDbContext _context;

        public EmployeeRepository(MacroDbContext context)
        {
            _context = context;
        }

        public string ExportToJson(int Id, string? FirstName, string? LastName, DateTime? HireDate, Boolean IsMarried, string? ManagerFirstName, string? ManagerLastName)
        {
            try
            {
                // Retrieve connection string from DbContext
                string connectionString = _context.Database.GetDbConnection().ConnectionString;

                using var conn = new SqlConnection(connectionString);
                conn.Open();

                // Create a command object
                using var command = new SqlCommand("udpExportEmployeeDataToJson", conn);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Id", Id != null ? Id : DBNull.Value);
                command.Parameters.AddWithValue("@FirstName", FirstName != null ? FirstName : DBNull.Value);
                command.Parameters.AddWithValue("@LastName", LastName != null ? LastName : DBNull.Value);
                command.Parameters.AddWithValue("@HireDate", HireDate != null ? HireDate : DBNull.Value);
                command.Parameters.AddWithValue("@IsMarried", IsMarried != null ? IsMarried : DBNull.Value);
                command.Parameters.AddWithValue("@ManagerFirstName", ManagerFirstName != null ? ManagerFirstName : DBNull.Value);
                command.Parameters.AddWithValue("@ManagerLAstName", ManagerLastName != null ? ManagerLastName : DBNull.Value);


                // Execute the command and retrieve JSON result
                var jsonResult = (string)command.ExecuteScalar();

                return jsonResult ?? ""; // Return JSON result or empty string if null
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error exporting data to JSON: {ex.Message}");
                return ""; // Return empty string in case of error
            }
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

        public string ExportToXml(int Id, string? FirstName, string? LastName, DateTime? HireDate, bool IsMarried, string? ManagerFirstName, string? ManagerLastName)
        {
            try
            {
                // Retrieve connection string from DbContext
                string connectionString = _context.Database.GetDbConnection().ConnectionString;

                using var conn = new SqlConnection(connectionString);
                conn.Open();

                // Create a command object
                using var command = new SqlCommand("udpExportEmployeeDataToXml", conn);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@FirstName", FirstName != null ? FirstName : DBNull.Value);
                command.Parameters.AddWithValue("@LastName", LastName != null ? LastName : DBNull.Value);
                command.Parameters.AddWithValue("@HireDate", HireDate != null ? HireDate : DBNull.Value);
                command.Parameters.AddWithValue("@IsMarried", IsMarried);
                command.Parameters.AddWithValue("@ManagerFirstName", ManagerFirstName != null ? ManagerFirstName : DBNull.Value);
                command.Parameters.AddWithValue("@ManagerLastName", ManagerLastName != null ? ManagerLastName : DBNull.Value);

                // Execute the command and retrieve XML result
                var xmlResult = (string)command.ExecuteScalar();

                return xmlResult ?? ""; // Return XML result or empty string if null
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error exporting data to XML: {ex.Message}");
                return ""; // Return empty string in case of error
            }
        }
    }
}
