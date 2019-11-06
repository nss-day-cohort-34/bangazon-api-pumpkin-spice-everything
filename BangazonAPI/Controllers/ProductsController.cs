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
    public class ProductsController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ProductsController(IConfiguration config)
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

        // GET api/producttypes


        [HttpGet]


        public async Task<IActionResult> GetAllProducts()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT p.Id, p.ProductName, p.Price, p.Description, p.Quantity, p.ProductTypeId,
                                        pt.TypeName,
                                        p.CustomerId AS SellerId,
                                        c.FirstName, c.LastName, c.CreationDate, c.LastActiveDate, c.IsActive
                                        FROM Product p INNER JOIN ProductType pt ON pt.Id = p.ProductTypeId
                                        LEFT JOIN Customer c ON c.Id = p.CustomerId";
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    Dictionary<int, Product> products = new Dictionary<int, Product>();
                    while (reader.Read())
                    {
                        int productId = reader.GetInt32(reader.GetOrdinal("Id"));
                        if (!products.ContainsKey(productId))
                        {
                            Product newProduct = new Product
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                ProductTypeId = reader.GetInt32(reader.GetOrdinal("ProductTypeId")),
                                ProductType = new ProductType()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("ProductTypeId")),
                                    TypeName = reader.GetString(reader.GetOrdinal("TypeName"))
                                },
                                CustomerId = reader.GetInt32(reader.GetOrdinal("SellerId")),
                                Customer = new Customer()
                                { 
                                    Id = reader.GetInt32(reader.GetOrdinal("SellerId")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    CreationDate = reader.GetDateTime(reader.GetOrdinal("CreationDate")),
                                    LastActiveDate = reader.GetDateTime(reader.GetOrdinal("LastActiveDate")),
                                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                                }
                            };
                            products.Add(productId, newProduct);
                        }

                        Product fromDictionary = products[productId];

                        if (!reader.IsDBNull(reader.GetOrdinal("ProductTypeId")))
                        {
                            ProductType aProductType = new ProductType()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("ProductTypeId")),
                                TypeName = reader.GetString(reader.GetOrdinal("TypeName"))                          
                            };
                            fromDictionary.ProductType = aProductType;
                        }
                    }

                    reader.Close();

                    return Ok(products.Values);
                }
            }
        }



        // GET api/producttypes/5
        [HttpGet("{id}")]

        public async Task<IActionResult> GetProduct([FromRoute] int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT p.Id AS ProductId, p.ProductName, p.Price, p.Description, p.Quantity, p.ProductTypeId,
                                        pt.TypeName,
                                        p.CustomerId AS SellerId,
                                        c.FirstName, c.LastName, c.CreationDate, c.LastActiveDate, c.IsActive
                                        FROM Product p INNER JOIN ProductType pt ON pt.Id = p.ProductTypeId
                                        LEFT JOIN Customer c ON c.Id = p.CustomerId
                                        WHERE p.Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    Dictionary<int, Product> products = new Dictionary<int, Product>();
                    while (reader.Read())
                    {
                        int productId = reader.GetInt32(reader.GetOrdinal("ProductId"));
                        if (!products.ContainsKey(productId))
                        {
                            Product newProduct = new Product
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                ProductTypeId = reader.GetInt32(reader.GetOrdinal("ProductTypeId")),
                                CustomerId = reader.GetInt32(reader.GetOrdinal("SellerId")),
                                Customer = new Customer()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("SellerId")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    CreationDate = reader.GetDateTime(reader.GetOrdinal("CreationDate")),
                                    LastActiveDate = reader.GetDateTime(reader.GetOrdinal("LastActiveDate")),
                                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                                }
                            };
                            products.Add(productId, newProduct);
                        }

                        Product fromDictionary = products[productId];

                        if (!reader.IsDBNull(reader.GetOrdinal("ProductTypeId")))
                        {
                            ProductType aProductType = new ProductType()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("ProductTypeId")),
                                TypeName = reader.GetString(reader.GetOrdinal("TypeName"))
                            };
                            fromDictionary.ProductType = aProductType;
                        }
                    }

                    reader.Close();

                    return Ok(products.Values);
                }
            }
        }



        // POST api/customers
        [HttpPost]
        public IActionResult Post([FromBody] Product product)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // More string interpolation
                    cmd.CommandText = @"
                        INSERT INTO Product (ProductName, Price, Description, Quantity, ProductTypeId, CustomerId)
                        OUTPUT INSERTED.Id
                        VALUES (@productName, @price, @description, @quantity, @productTypeId, @customerId)";
                    cmd.Parameters.Add(new SqlParameter("@productName", product.ProductName));
                    cmd.Parameters.Add(new SqlParameter("@price", product.Price));
                    cmd.Parameters.Add(new SqlParameter("@description", product.Description));
                    cmd.Parameters.Add(new SqlParameter("@quantity", product.Quantity));
                    cmd.Parameters.Add(new SqlParameter("@productTypeId", product.ProductTypeId));
                    cmd.Parameters.Add(new SqlParameter("@customerId", product.CustomerId));

                    cmd.ExecuteNonQuery();

                    return Created("api/product", product);
                }
            }
        }

        // PUT api/customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Product product)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                            UPDATE Product
                            SET ProductName = @productName, Price = @price, 
                            Description = @description, Quantity = @quantity, 
                            ProductTypeId = @productTypeId, CustomerId = @customerId
                            WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", product.Id));
                        cmd.Parameters.Add(new SqlParameter("@productName", product.ProductName));
                        cmd.Parameters.Add(new SqlParameter("@price", product.Price));
                        cmd.Parameters.Add(new SqlParameter("@description", product.Description));
                        cmd.Parameters.Add(new SqlParameter("@quantity", product.Quantity));
                        cmd.Parameters.Add(new SqlParameter("@productTypeId", product.ProductTypeId));
                        cmd.Parameters.Add(new SqlParameter("@customerId", product.CustomerId));

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
                if (!ProductExists(id))
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
                                        FROM Product 
                                        WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private bool ProductExists(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id FROM Product WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();

                    return reader.Read();
                }
            }
        }
    }
}