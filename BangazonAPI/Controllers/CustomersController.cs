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
    public class CustomersController : ControllerBase
    {
        private readonly IConfiguration _config;

        public CustomersController(IConfiguration config)
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
        public async Task<IActionResult> Get(string q, string include)
        {
            if (q != null && include != null)
            {
                return await GetAllCustomersWithProductsQ(q, include);
            }
            else if (q != null && include == null)
            {
                return await GetAllCustomersWithQ(q);
            }
            else if (q == null && include != null)
            {
                return await GetAllCustomersWithProducts(include);
            }
            else
            {
                return await GetAllCustomers();
            }
        }

        public async Task<IActionResult> GetAllCustomers()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, FirstName, LastName, CreationDate, LastActiveDate, IsActive
                                          FROM Customer";
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    Dictionary<int, Customer> customers = new Dictionary<int, Customer>();
                    while (reader.Read())
                    {
                        int customerId = reader.GetInt32(reader.GetOrdinal("Id"));
                        Customer newCustomer = new Customer
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            CreationDate = reader.GetDateTime(reader.GetOrdinal("CreationDate")),
                            LastActiveDate = reader.GetDateTime(reader.GetOrdinal("LastActiveDate")),
                            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                        };

                        customers.Add(customerId, newCustomer);
                    }

                    reader.Close();

                    return Ok(customers.Values);
                }
            }
        }

        //Get All customers with Q query
        public async Task<IActionResult> GetAllCustomersWithQ(string q)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, FirstName, LastName, CreationDate, LastActiveDate, IsActive
                                          FROM Customer
                                         WHERE FirstName LIKE @q OR LastName LIKE @q OR CreationDate LIKE @q OR LastActiveDate LIKE @q OR IsActive LIKE @q";
                    cmd.Parameters.Add(new SqlParameter("@q", $"%{q}%"));

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    List<Customer> customers = new List<Customer>();
                    while (reader.Read())
                    {
                        Customer customer = new Customer
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            CreationDate = reader.GetDateTime(reader.GetOrdinal("CreationDate")),
                            LastActiveDate = reader.GetDateTime(reader.GetOrdinal("LastActiveDate")),
                            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                        };

                        customers.Add(customer);
                    }

                    reader.Close();

                    return Ok(customers);
                }
            }
        }

        //Get all customers with Q and Include products
        public async Task<IActionResult> GetAllCustomersWithProductsQ(string q, string include)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                   
                    if (include.ToLower() == "product")
                    {
                        cmd.CommandText = @"SELECT c.Id as CustomerId, c.FirstName, c.LastName, c.CreationDate, c.LastActiveDate, c.IsActive,
                                                   p.Id as ProductId, p.ProductName, p.ProductTypeId as ProductTypeId, p.Price, p.Quantity, p.Description
                                          FROM Customer c LEFT JOIN Product p ON c.Id = p.CustomerId
                                         WHERE c.FirstName LIKE @q OR c.LastName LIKE @q OR c.CreationDate LIKE @q OR c.LastActiveDate LIKE @q OR c.IsActive LIKE @q";
                        cmd.Parameters.Add(new SqlParameter("@q", $"%{q}%"));
                        SqlDataReader reader = cmd.ExecuteReader();
                                               
                        Dictionary<int, Customer> customers = new Dictionary<int, Customer>();
                        while (reader.Read())
                        {
                            int customerId = reader.GetInt32(reader.GetOrdinal("CustomerId"));
                            if (!customers.ContainsKey(customerId))
                            {
                                Customer customer = new Customer
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    CreationDate = reader.GetDateTime(reader.GetOrdinal("CreationDate")),
                                    LastActiveDate = reader.GetDateTime(reader.GetOrdinal("LastActiveDate")),
                                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                };

                                customers.Add(customerId, customer);
                            }
                            Customer fromDictionary = customers[customerId];

                            if (!reader.IsDBNull(reader.GetOrdinal("ProductId")))
                            {
                                Product product = new Product
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                    ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                    Description = reader.GetString(reader.GetOrdinal("Description")),
                                    Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                    ProductTypeId = reader.GetInt32(reader.GetOrdinal("ProductTypeId")),

                                };
                                fromDictionary.ProductsToSell.Add(product);
                            }
                        }

                        reader.Close();
                        return Ok(customers.Values);
                    }
                    
                }
                return null;
                }
            }

        //Get all customers with Include products
        public async Task<IActionResult> GetAllCustomersWithProducts(string include)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {

                    if (include.ToLower() == "product")
                    {
                        cmd.CommandText = @"SELECT c.Id as CustomerId, c.FirstName, c.LastName, c.CreationDate, c.LastActiveDate, c.IsActive,
                                                   p.Id as ProductId, p.ProductName, p.ProductTypeId as ProductTypeId, p.Price, p.Quantity, p.Description
                                          FROM Customer c LEFT JOIN Product p ON c.Id = p.CustomerId";
                        SqlDataReader reader = cmd.ExecuteReader();

                        Dictionary<int, Customer> customers = new Dictionary<int, Customer>();
                        while (reader.Read())
                        {
                            int customerId = reader.GetInt32(reader.GetOrdinal("CustomerId"));
                            if (!customers.ContainsKey(customerId))
                            {
                                Customer customer = new Customer
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    CreationDate = reader.GetDateTime(reader.GetOrdinal("CreationDate")),
                                    LastActiveDate = reader.GetDateTime(reader.GetOrdinal("LastActiveDate")),
                                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                };

                                customers.Add(customerId, customer);
                            }
                            Customer fromDictionary = customers[customerId];

                            if (!reader.IsDBNull(reader.GetOrdinal("ProductId")))
                            {
                                Product product = new Product
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                    ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                    Description = reader.GetString(reader.GetOrdinal("Description")),
                                    Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                    ProductTypeId = reader.GetInt32(reader.GetOrdinal("ProductTypeId")),

                                };
                                fromDictionary.ProductsToSell.Add(product);
                            }
                        }

                        reader.Close();
                        return Ok(customers.Values);
                    }

                }
                return null;
            }
        }


        // GET api/customers/5
        [HttpGet("{id}")]

        public async Task<IActionResult> Get([FromRoute] int id, string include)
        {
            if (include != null)
            {
                return await GetCustomersWithProducts(id, include);
            }
            else
            {
                return await GetCustomers(id);
            }
        }
        public async Task<IActionResult> GetCustomers([FromRoute] int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, FirstName, LastName, CreationDate, LastActiveDate, IsActive
                                          FROM Customer
                                        WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    Customer customer = null;
                    if (reader.Read())
                    {
                        customer = new Customer
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            CreationDate = reader.GetDateTime(reader.GetOrdinal("CreationDate")),
                            LastActiveDate = reader.GetDateTime(reader.GetOrdinal("LastActiveDate")),
                            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                        };
                    }

                    reader.Close();

                    return Ok(customer);
                }
            }
        }

        public async Task<IActionResult> GetCustomersWithProducts(int id, string include)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {

                    if (include.ToLower() == "product")
                    {
                        cmd.CommandText = @"SELECT c.Id as CustomerId, c.FirstName, c.LastName, c.CreationDate, c.LastActiveDate, c.IsActive,
                                                   p.Id as ProductId, p.ProductName, p.ProductTypeId as ProductTypeId, p.Price, p.Quantity, p.Description
                                          FROM Customer c LEFT JOIN Product p ON c.Id = p.CustomerId
                                         WHERE c.Id = @id";
                      
                        SqlDataReader reader = cmd.ExecuteReader();

                        Dictionary<int, Customer> customers = new Dictionary<int, Customer>();
                        while (reader.Read())
                        {
                            int customerId = reader.GetInt32(reader.GetOrdinal("CustomerId"));
                            if (!customers.ContainsKey(customerId))
                            {
                                Customer customer = new Customer
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    CreationDate = reader.GetDateTime(reader.GetOrdinal("CreationDate")),
                                    LastActiveDate = reader.GetDateTime(reader.GetOrdinal("LastActiveDate")),
                                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                };

                                customers.Add(customerId, customer);
                            }
                            Customer fromDictionary = customers[customerId];

                            if (!reader.IsDBNull(reader.GetOrdinal("ProductId")))
                            {
                                Product product = new Product
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                    ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                    Description = reader.GetString(reader.GetOrdinal("Description")),
                                    Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                    ProductTypeId = reader.GetInt32(reader.GetOrdinal("ProductTypeId")),

                                };
                                fromDictionary.ProductsToSell.Add(product);
                            }
                        }

                        reader.Close();
                        return Ok(customers.Values);
                    }

                }
                return null;
            }
        }

        // POST api/customers
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Customer customer)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // More string interpolation
                    cmd.CommandText = @"
                        INSERT INTO Customer (FirstName, LastName, CreationDate, LastActiveDate, IsActive)
                        OUTPUT INSERTED.Id
                        VALUES (@firstName, @lastName, @creationDate, @lastActiveDate, @isActive)
                    ";
                    cmd.Parameters.Add(new SqlParameter("@firstName", customer.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@lastName", customer.LastName));
                    cmd.Parameters.Add(new SqlParameter("@creationDate", customer.CreationDate));
                    cmd.Parameters.Add(new SqlParameter("@lastActiveDate", customer.LastActiveDate));
                    cmd.Parameters.Add(new SqlParameter("@isActive", customer.IsActive));


                    customer.Id = (int) await cmd.ExecuteScalarAsync();

                    return CreatedAtRoute("GetCustomer", new { id = customer.Id }, customer);
                }
            }
        }

        // PUT api/customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Customer customer)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                            UPDATE Customer
                            SET FirstName = @firstName, LastName = @lastName, CreationDate = @creationDate, LastActiveDate = @lastActiveDate, IsActive = @isActive
                            WHERE Id = @id
                        ";
                        cmd.Parameters.Add(new SqlParameter("@id", customer.Id));
                        cmd.Parameters.Add(new SqlParameter("@firstName", customer.FirstName));
                        cmd.Parameters.Add(new SqlParameter("@lastName", customer.LastName));
                        cmd.Parameters.Add(new SqlParameter("@creationDate", customer.CreationDate));
                        cmd.Parameters.Add(new SqlParameter("@lastActiveDate", customer.LastActiveDate));
                        cmd.Parameters.Add(new SqlParameter("@isActive", customer.IsActive));

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
                if (!CustomerExists(id))
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
        public async Task<IActionResult> Delete(int id)
        {
            throw new NotImplementedException("This method isn't implemented...yet.");
        }

        private bool CustomerExists(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id FROM Customer WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();

                    return reader.Read();
                }
            }
        }
    }
}
