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
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _config;

        public EmployeeController(IConfiguration config)
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


        // GET: api/Employee ---code for get list of employees with department and computer
        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                    SELECT e.Id AS EmployeeId, e.FirstName, e.LastName, e.DepartmentId, 
                                          e.IsSupervisor, e.StartDate, d.Id AS DepartmentId, d.Name AS DepartmentName, d.Budget, 
                                          c.Id AS ComputerId, c.Manufacturer, c.Make, c.PurchaseDate, c.DecomissionDate, 
                                          ce.Id AS ComputerEmployeeId
                                          FROM Employee e 
                                          LEFT JOIN Department d ON e.DepartmentId = d.Id
                                          LEFT JOIN ComputerEmployee ce ON ce.EmployeeId = e.Id 
                                          LEFT JOIN Computer c ON ce.ComputerId = c.Id";
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    Dictionary<int, Employee> employees = new Dictionary<int, Employee>();
                    Dictionary<int, Computer> computers = new Dictionary<int, Computer>();

                    while (reader.Read())
                    {
                        int employeeId = reader.GetInt32(reader.GetOrdinal("EmployeeId")); //get the id
                        if (!employees.ContainsKey(employeeId)) //have I seen this employee before?
                        {
                            Employee newEmployee = new Employee() //() if it does not..then lets create it.
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                                IsSupervisor = reader.GetBoolean(reader.GetOrdinal("IsSupervisor")),
                                StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                                Department = new Department()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                                    Name = reader.GetString(reader.GetOrdinal("DepartmentName")),
                                    Budget = reader.GetInt32(reader.GetOrdinal("Budget"))
                                }
                            };
                            employees.Add(employeeId, newEmployee);
                        }

                        Employee fromDictionary = employees[employeeId];

                        if (!reader.IsDBNull(reader.GetOrdinal("ComputerId"))) //if i have an computer id then it will run.
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal("DecomissionDate")))
                            {
                                Computer newComputer = new Computer()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("ComputerId")),
                                    Make = reader.GetString(reader.GetOrdinal("Make")),
                                    Manufacturer = reader.GetString(reader.GetOrdinal("Manufacturer")),
                                    PurchaseDate = reader.GetDateTime(reader.GetOrdinal("PurchaseDate")),
                                    DecommissionDate = reader.GetDateTime(reader.GetOrdinal("DecomissionDate"))
                                };
                                fromDictionary.Computer = newComputer;
                            }
                            else
                            {
                                Computer newComputer = new Computer()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("ComputerId")),
                                    Make = reader.GetString(reader.GetOrdinal("Make")),
                                    Manufacturer = reader.GetString(reader.GetOrdinal("Manufacturer")),
                                    PurchaseDate = reader.GetDateTime(reader.GetOrdinal("PurchaseDate")),
                                };
                                fromDictionary.Computer = newComputer;
                            }
                        }
                        /*
                            The Ok() method is an abstraction that constructs
                            a new HTTP response with a 200 status code, and converts
                            your IEnumerable into a JSON string to be sent back to
                            the requessting client application.
                        */
                    }
                    reader.Close();
                    return Ok(employees.Values);
                }
            }
        }

        // GET: api/Employee/1 ---code for get one employee
        [HttpGet("{id}", Name = "GetEmployee")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"  
                                       SELECT e.Id AS EmployeeId, e.FirstName, e.LastName, e.DepartmentId, 
                                          e.IsSupervisor, e.StartDate, d.Id AS DepartmentId, d.Name AS DepartmentName, d.Budget, 
                                          c.Id AS ComputerId, c.Manufacturer, c.Make, c.PurchaseDate, c.DecomissionDate, 
                                          ce.Id AS ComputerEmployeeId
                                          FROM Employee e 
                                          LEFT JOIN Department d ON e.DepartmentId = d.Id
                                          LEFT JOIN ComputerEmployee ce ON ce.EmployeeId = e.Id 
                                          LEFT JOIN Computer c ON ce.ComputerId = c.Id
                                          WHERE EmployeeId = @id";

                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    Employee newEmployee = null;
                    if (reader.Read())
                    {
                        newEmployee = new Employee() //() if it does not..then lets create it.
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                            IsSupervisor = reader.GetBoolean(reader.GetOrdinal("IsSupervisor")),
                            StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                            Department = new Department()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                                Name = reader.GetString(reader.GetOrdinal("DepartmentName")),
                                Budget = reader.GetInt32(reader.GetOrdinal("Budget"))
                            },
                            Computer = new Computer()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("ComputerId")),
                                Make = reader.GetString(reader.GetOrdinal("Make")),
                                Manufacturer = reader.GetString(reader.GetOrdinal("Manufacturer")),
                                PurchaseDate = reader.GetDateTime(reader.GetOrdinal("PurchaseDate")),
                            }


                        };
                    }


                    reader.Close();
                    return Ok(newEmployee);
                }

            }
        }

        // POST: api/Employee
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Employee/5
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
