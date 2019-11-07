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
    public class ProductTypesController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ProductTypesController(IConfiguration config)
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
        

        public async Task<IActionResult> GetAllProductTypes()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT pt.Id, pt.TypeName, 
                                          p.Id AS ProductId, p.ProductName, p.Price, p.Description, p.Quantity, p.CustomerId, p.ProductTypeId
                                          FROM ProductType pt INNER JOIN Product p ON p.ProductTypeId = pt.Id";
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    Dictionary<int, ProductType> productTypes = new Dictionary<int, ProductType>();
                    while (reader.Read())
                    {
                        int productTypeId = reader.GetInt32(reader.GetOrdinal("Id"));
                        if (!productTypes.ContainsKey(productTypeId))
                        {
                            ProductType newProductType = new ProductType
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                TypeName = reader.GetString(reader.GetOrdinal("TypeName")),
                            };
                        productTypes.Add(productTypeId, newProductType);
                        }

                        ProductType fromDictionary = productTypes[productTypeId];

                        if (!reader.IsDBNull(reader.GetOrdinal("ProductId")))
                        {
                            Product aProduct = new Product()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                ProductTypeId = reader.GetInt32(reader.GetOrdinal("ProductTypeId"))
                            };
                            fromDictionary.Products.Add(aProduct);
                        }
                    }

                    reader.Close();

                    return Ok(productTypes.Values);
                }
            }
        }

      

        // GET api/producttypes/5
        [HttpGet("{id}")]

        public async Task<IActionResult> GetProductType([FromRoute] int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT pt.Id, pt.TypeName,
                                        p.Id AS ProductId, p.ProductName, p.Price, p.Description, p.Quantity, p.CustomerId, p.ProductTypeId
                                          FROM ProductType pt INNER JOIN Product p ON p.ProductTypeId = pt.Id
                                        WHERE pt.Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    Dictionary<int, ProductType> productTypes = new Dictionary<int, ProductType>();
                    while (reader.Read())
                    {
                        int productTypeId = reader.GetInt32(reader.GetOrdinal("Id"));
                        if (!productTypes.ContainsKey(productTypeId))
                        {
                            ProductType newProductType = new ProductType
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                TypeName = reader.GetString(reader.GetOrdinal("TypeName")),
                            };
                            productTypes.Add(productTypeId, newProductType);
                        }

                        ProductType fromDictionary = productTypes[productTypeId];

                        if (!reader.IsDBNull(reader.GetOrdinal("ProductId")))
                        {
                            Product aProduct = new Product()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                ProductTypeId = reader.GetInt32(reader.GetOrdinal("ProductTypeId"))
                            };
                            fromDictionary.Products.Add(aProduct);
                        }
                    }


                    reader.Close();

                    return Ok(productTypes.Values.First());
                }
            }
        }

       

        // POST api/customers
        [HttpPost]
        public IActionResult Post([FromBody] ProductType productType)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // More string interpolation
                    cmd.CommandText = @"
                        INSERT INTO ProductType (TypeName)
                        OUTPUT INSERTED.Id
                        VALUES (@typeName)";
                    cmd.Parameters.Add(new SqlParameter("@typeName", productType.TypeName));         

                    cmd.ExecuteNonQuery();

                    return Created("api/productType", productType);
                }
            }
        }

        // PUT api/customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProductType productType)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                            UPDATE ProductType
                            SET TypeName = @typeName
                            WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", productType.Id));
                        cmd.Parameters.Add(new SqlParameter("@typeName", productType.TypeName));
                        

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
                if (!ProductTypeExists(id))
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
                                        FROM ProductType 
                                        WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private bool ProductTypeExists(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id FROM ProductType WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();

                    return reader.Read();
                }
            }
        }
    }
}
