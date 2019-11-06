using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BangazonAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BangazonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _config;

        public DepartmentController(IConfiguration config)
        {
            _config = config;
        }

        private SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }
        // GET: api/Department ----code for list of departments
        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT d.Id AS DepartmentId, d.Name, d.Budget, e.Id AS EmployeeId, e.FirstName, e.LastName, e.DepartmentId, e.IsSupervisor, e.StartDate
                                          FROM Department d LEFT JOIN Employee e ON e.departmentId = d.Id";

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    Dictionary<int, Department> deparments = new Dictionary<int, Department>();
                    Dictionary<int, Employee> employees = new Dictionary<int, Employee>();

                    while (reader.Read())
                    {
                        int departmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId"));
                        int employeeId = reader.GetInt32(reader.GetOrdinal("EmployeeId"));

                        if (!deparments.ContainsKey(departmentId)) //have I seen this department before?
                        {
                            Department newDepartment = new Department
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Budget = reader.GetInt32(reader.GetOrdinal("Budget")),
                            };

                            deparments.Add(departmentId, newDepartment);
                        }
                        Department fromDictionary = deparments[departmentId];
                        if (!reader.IsDBNull(reader.GetOrdinal("EmployeeId")))
                        {
                            if (!employees.ContainsKey(employeeId))
                            {
                                Employee newEmployee = new Employee
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                                    IsSupervisor = reader.GetBoolean(reader.GetOrdinal("IsSupervisor")),
                                    StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                                };
                                employees.Add(employeeId, newEmployee);
                                fromDictionary.Employees.Add(newEmployee);
                            }
                        }
                    }

                        reader.Close();

                        return Ok(deparments.Values);
                    }
                }
            }
        

        // GET: api/Department/5
        [HttpGet("{id}", Name = "GetDept")]
        public async Task<IActionResult> GetDepartment(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, Name, Budget
                                        FROM Department
                                        WHERE Id = @id";

                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    Department newDepartment = null;
                    if (reader.Read())
                    {
                        newDepartment = new Department
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Budget = reader.GetInt32(reader.GetOrdinal("Budget"))
                        };
                    }

                    reader.Close();

                    return Ok(newDepartment);
                }
            }
        }

        // POST: ---Create a department
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Department department)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // More string interpolation
                    cmd.CommandText = @"
                        INSERT INTO Department (Name, Budget)
                        OUTPUT INSERTED.Id
                        VALUES (@name, @budget)
                    ";
                    cmd.Parameters.Add(new SqlParameter("@name", department.Name));
                    cmd.Parameters.Add(new SqlParameter("@budget", department.Budget));

                    department.Id = (int)await cmd.ExecuteScalarAsync();

                    return CreatedAtRoute("GetDepartment", new { id = department.Id }, department);
                }
            }
        }

        // PUT: api/Department/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
