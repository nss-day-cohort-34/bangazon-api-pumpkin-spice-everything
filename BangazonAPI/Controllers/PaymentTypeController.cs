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
    public class PaymentTypeController : ControllerBase
    {
        private readonly IConfiguration _config;

        public PaymentTypeController(IConfiguration config)
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


        // GET: api/PaymentType
        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, AcctNumber, Type, CustomerId
                                          FROM PaymentType";
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    Dictionary<int, PaymentType> paymenttypes = new Dictionary<int, PaymentType>();
                    while (reader.Read())
                    {
                        int paymentTypeId = reader.GetInt32(reader.GetOrdinal("Id"));
                        PaymentType newPaymentType = new PaymentType
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            AcctNumber = reader.GetString(reader.GetOrdinal("AcctNumber")),
                            Type = reader.GetString(reader.GetOrdinal("Type")),
                            CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                        };

                        paymenttypes.Add(paymentTypeId, newPaymentType);
                    }

                    reader.Close();

                    return Ok(paymenttypes.Values);
                }
            }
        }

        // GET: api/PaymentType/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/PaymentType
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/PaymentType/5
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
