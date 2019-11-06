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

        [HttpGet]

        public async Task<IActionResult> Get(string filter, string include)
        {
            if (filter != null)
            {
                return await GetAllDepartmentsWithBudget(filter);
            }
            else if (include != null)
            {
                return await GetAllDepartmentsWithEmployees(include);
            }
            else
            {
                return await GetAllDepartments();
            }
        }

        // GET: api/Department ----code for list of departments
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
                        int employeeId = reader.GetInt32(reader.GetOrdinal("EmployeeId"));
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
        // code for get all departments with employees using include query
        public async Task<IActionResult> GetAllDepartmentsWithEmployees(string include)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    if (include.ToLower() == "employee")
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
                return null;
            }
        }

        // code for get all departments using budget query
        public async Task<IActionResult> GetAllDepartmentsWithBudget(string filter)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    if (filter.ToLower() == "budget&_gt=300000")
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
                return null;
            }
        }

        // GET: api/Department/1   -----code to get a single department without employees
        [HttpGet("{id}", Name = "GetDepartment")]
        public async Task<IActionResult> GetDepartment(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT d.Id AS DepartmentId, d.Name, d.Budget, e.Id AS EmployeeId, 
                                                e.FirstName, e.LastName, e.DepartmentId, e.IsSupervisor, e.StartDate
                                        FROM Department d LEFT JOIN Employee e ON e.departmentId = d.Id
                                        WHERE DepartmentId = @id";

                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    Department newDepartment = null;
                    if (reader.Read())
                    {
                        newDepartment = new Department
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
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
        public async Task<IActionResult> Put(int id, [FromBody] Department department)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                            UPDATE Department
                            SET Name = @name, Budget = @budget
                            WHERE Id = @id
                        ";
                        cmd.Parameters.Add(new SqlParameter("@id", department.Id));
                        cmd.Parameters.Add(new SqlParameter("@name", department.Name));
                        cmd.Parameters.Add(new SqlParameter("@budget", department.Budget));
                        

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            return new StatusCodeResult(StatusCodes.Status204NoContent);
                        }

                        throw new Exception("No rows affected");
                    }
                }
            }
            catch (Exception)
            {
                if (!DepartmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // DELETE: api/ApiWithActions/5 NOT ON TICKET
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}


        //method to check if resource exists
        private bool DepartmentExists(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id FROM Department WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();

                    return reader.Read();
                }
            }
        }
    }
}
