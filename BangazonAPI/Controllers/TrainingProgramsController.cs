using System;
using System.Collections.Generic;
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
    public class TrainingProgramsController : ControllerBase
    {
        private readonly IConfiguration _config;

        public TrainingProgramsController(IConfiguration config)
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

        // GET api/customers
        [HttpGet]
        public async Task<IActionResult> Get(string complete)
        {
            if (complete == "false")
            {
                return await GetFutureTrainingPrograms();
            } 
            else if (complete == "true")
            {
                return await GetCompletedTrainingPrograms();
            }  
            else 
            {
                return await GetAllTrainingPrograms();
            }
        }

        public async Task<IActionResult> GetAllTrainingPrograms()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT tp.Id AS TPId, tp.ProgramName, tp.StartDate AS ProgramStartDate, tp.EndDate, tp.MaxAttendees, 
                                       et.EmployeeId, e.FirstName, e.LastName, e.DepartmentId, e.IsSupervisor, e.StartDate
                                          FROM TrainingProgram tp LEFT JOIN EmployeeTraining et ON et.TrainingProgramId = tp.Id
                                          LEFT JOIN Employee e ON e.Id = et.EmployeeId";
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    Dictionary<int, TrainingProgram> trainingPrograms = new Dictionary<int, TrainingProgram>();
                    while (reader.Read())
                    {
                        int trainingProgramId = reader.GetInt32(reader.GetOrdinal("TPId"));
                        if (!trainingPrograms.ContainsKey(trainingProgramId))
                        {
                            TrainingProgram newTrainingProgram = new TrainingProgram
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("TPId")),
                                ProgramName = reader.GetString(reader.GetOrdinal("ProgramName")),
                                StartDate = reader.GetDateTime(reader.GetOrdinal("ProgramStartDate")),
                                EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")),
                                MaxAttendees = reader.GetInt32(reader.GetOrdinal("MaxAttendees")),
                            };

                            trainingPrograms.Add(trainingProgramId, newTrainingProgram);
                        }

                        TrainingProgram fromDictionary = trainingPrograms[trainingProgramId];

                        if (!reader.IsDBNull(reader.GetOrdinal("EmployeeId")))
                        {
                            Employee anEmployee = new Employee()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                                IsSupervisor = reader.GetBoolean(reader.GetOrdinal("IsSupervisor")),
                                StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate"))
                            };

                            fromDictionary.CurrentAttendees.Add(anEmployee);
                        }
                    }

                    reader.Close();

                    return Ok(trainingPrograms.Values);
                }
            }
        }

        //Get All customers with Q query
        public async Task<IActionResult> GetFutureTrainingPrograms()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT tp.Id, tp.ProgramName, tp.StartDate AS ProgramStartDate, tp.EndDate, tp.MaxAttendees, 
                                       et.EmployeeId, e.FirstName, e.LastName, e.DepartmentId, e.IsSupervisor, e.StartDate
                                          FROM TrainingProgram tp LEFT JOIN EmployeeTraining et ON et.TrainingProgramId = tp.Id
                                          LEFT JOIN Employee e ON e.Id = et.EmployeeId
                                          WHERE tp.StartDate > GetDate()";

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    Dictionary<int, TrainingProgram> trainingPrograms = new Dictionary<int, TrainingProgram>();
                    while (reader.Read())
                    {
                        int trainingProgramId = reader.GetInt32(reader.GetOrdinal("Id"));
                        if (!trainingPrograms.ContainsKey(trainingProgramId))
                        {
                            TrainingProgram newTrainingProgram = new TrainingProgram
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                ProgramName = reader.GetString(reader.GetOrdinal("ProgramName")),
                                StartDate = reader.GetDateTime(reader.GetOrdinal("ProgramStartDate")),
                                EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")),
                                MaxAttendees = reader.GetInt32(reader.GetOrdinal("MaxAttendees")),
                            };

                            trainingPrograms.Add(trainingProgramId, newTrainingProgram);
                        }

                        TrainingProgram fromDictionary = trainingPrograms[trainingProgramId];

                        if (!reader.IsDBNull(reader.GetOrdinal("EmployeeId")))
                        {
                            Employee anEmployee = new Employee()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                                IsSupervisor = reader.GetBoolean(reader.GetOrdinal("IsSupervisor")),
                                StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate"))
                            };
                            fromDictionary.CurrentAttendees.Add(anEmployee);
                        }
                    }

                    reader.Close();

                    return Ok(trainingPrograms.Values);
                }
            }
        }

        //Get all customers with Q and Include products
        public async Task<IActionResult> GetCompletedTrainingPrograms()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT tp.Id, tp.ProgramName, tp.StartDate AS ProgramStartDate, tp.EndDate, tp.MaxAttendees, 
                                       et.EmployeeId, e.FirstName, e.LastName, e.DepartmentId, e.IsSupervisor, e.StartDate
                                          FROM TrainingProgram tp LEFT JOIN EmployeeTraining et ON et.TrainingProgramId = tp.Id
                                          LEFT JOIN Employee e ON e.Id = et.EmployeeId
                                          WHERE tp.EndDate < GetDate()";

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    Dictionary<int, TrainingProgram> trainingPrograms = new Dictionary<int, TrainingProgram>();
                    while (reader.Read())
                    {
                        int trainingProgramId = reader.GetInt32(reader.GetOrdinal("Id"));
                        if (!trainingPrograms.ContainsKey(trainingProgramId))
                        {
                            TrainingProgram newTrainingProgram = new TrainingProgram
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                ProgramName = reader.GetString(reader.GetOrdinal("ProgramName")),
                                StartDate = reader.GetDateTime(reader.GetOrdinal("ProgramStartDate")),
                                EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")),
                                MaxAttendees = reader.GetInt32(reader.GetOrdinal("MaxAttendees")),
                            };

                            trainingPrograms.Add(trainingProgramId, newTrainingProgram);
                        }

                        TrainingProgram fromDictionary = trainingPrograms[trainingProgramId];

                        if (!reader.IsDBNull(reader.GetOrdinal("EmployeeId")))
                        {
                            Employee anEmployee = new Employee()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                                IsSupervisor = reader.GetBoolean(reader.GetOrdinal("IsSupervisor")),
                                StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate"))
                            };
                            fromDictionary.CurrentAttendees.Add(anEmployee);
                        }
                    }

                    reader.Close();

                    return Ok(trainingPrograms.Values);
                }
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute]int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT tp.Id, tp.ProgramName, tp.StartDate AS ProgramStartDate, tp.EndDate, tp.MaxAttendees, 
                                       et.EmployeeId, e.FirstName, e.LastName, e.DepartmentId, e.IsSupervisor, e.StartDate
                                          FROM TrainingProgram tp LEFT JOIN EmployeeTraining et ON et.TrainingProgramId = tp.Id
                                          LEFT JOIN Employee e ON e.Id = et.EmployeeId
                                          WHERE tp.Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    Dictionary<int, TrainingProgram> trainingPrograms = new Dictionary<int, TrainingProgram>();
                    while (reader.Read())
                    {
                        int trainingProgramId = reader.GetInt32(reader.GetOrdinal("Id"));
                        if (!trainingPrograms.ContainsKey(trainingProgramId))
                        {
                            TrainingProgram newTrainingProgram = new TrainingProgram
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                ProgramName = reader.GetString(reader.GetOrdinal("ProgramName")),
                                StartDate = reader.GetDateTime(reader.GetOrdinal("ProgramStartDate")),
                                EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")),
                                MaxAttendees = reader.GetInt32(reader.GetOrdinal("MaxAttendees")),
                            };

                            trainingPrograms.Add(trainingProgramId, newTrainingProgram);
                        }

                        TrainingProgram fromDictionary = trainingPrograms[trainingProgramId];

                        if (!reader.IsDBNull(reader.GetOrdinal("EmployeeId")))
                        {
                            Employee anEmployee = new Employee()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                                IsSupervisor = reader.GetBoolean(reader.GetOrdinal("IsSupervisor")),
                                StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate"))
                            };
                            fromDictionary.CurrentAttendees.Add(anEmployee);
                        }
                    }

                    reader.Close();

                    return Ok(trainingPrograms.Values.First());
                }
            }
        }


        // POST api/customers
        [HttpPost]
        public IActionResult Post([FromBody] TrainingProgram trainingProgram)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // More string interpolation
                    cmd.CommandText = @"
                        INSERT INTO TrainingProgram (ProgramName, StartDate, EndDate, MaxAttendees)
                        OUTPUT INSERTED.Id
                        VALUES (@programName, @startDate, @endDate, @maxAttendees)";
                    cmd.Parameters.Add(new SqlParameter("@programName", trainingProgram.ProgramName));
                    cmd.Parameters.Add(new SqlParameter("@startDate", trainingProgram.StartDate));
                    cmd.Parameters.Add(new SqlParameter("@endDate", trainingProgram.EndDate));
                    cmd.Parameters.Add(new SqlParameter("@maxAttendees", trainingProgram.MaxAttendees));

                    cmd.ExecuteNonQuery();

                    return Created("api/trainingprogram", trainingProgram);
                }
            }
        }

       
        // PUT: api/Department/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute]int id, [FromBody] TrainingProgram trainingProgram)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                            UPDATE TrainingProgram
                            SET ProgramName = @programName, StartDate = @startDate, EndDate = @endDate, MaxAttendees = @maxAttendees
                            WHERE Id = @id
                        ";
                        cmd.Parameters.Add(new SqlParameter("@id", trainingProgram.Id));
                        cmd.Parameters.Add(new SqlParameter("@programName", trainingProgram.ProgramName));
                        cmd.Parameters.Add(new SqlParameter("@startDate", trainingProgram.StartDate));
                        cmd.Parameters.Add(new SqlParameter("@endDate", trainingProgram.EndDate));
                        cmd.Parameters.Add(new SqlParameter("@maxAttendees", trainingProgram.MaxAttendees));



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
                if (!TrainingProgramExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }


        // DELETE api/customers/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE 
                                        FROM TrainingProgram 
                                        WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private bool TrainingProgramExists(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id FROM TrainingProgram WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();

                    return reader.Read();
                }
            }
        }
    }
}