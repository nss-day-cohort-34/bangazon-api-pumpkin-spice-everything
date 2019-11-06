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
    public class OrderController : ControllerBase
    {
        private readonly IConfiguration _config;

        public OrderController(IConfiguration config)
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
        public async Task<IActionResult> Get(string q, string include)
        {
         if (include != null)
            {
                return await GetAllOrdersWithProducts(include);
            }
            else
            {
                return await GetAllOrders();
            }
        }

        // GET api/customers
        public async Task<IActionResult> GetAllOrders()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, CustomerId, PaymentTypeId, OrderDate
                                          FROM [Order]";
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    Dictionary<int, Order> orders = new Dictionary<int, Order>();
                    while (reader.Read())
                    {
                        int orderId = reader.GetInt32(reader.GetOrdinal("Id"));
                        Order newOrder = new Order
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                            PaymentTypeId = reader.GetInt32(reader.GetOrdinal("PaymentTypeId")),
                            OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                        };

                        orders.Add(orderId, newOrder);
                    }

                    reader.Close();

                    return Ok(orders.Values);
                }
            }
        }

        [HttpGet("{id}", Name = "GetOrder")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT
                            Id, CustomerId, PaymentTypeId, OrderDate
                        FROM [Order]
                        WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Order order = null;

                    if (reader.Read())
                    {
                        order = new Order
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                            PaymentTypeId = reader.GetInt32(reader.GetOrdinal("PaymentTypeId")),
                            OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate"))
                        };
                    }
                    reader.Close();

                    return Ok(order);
                }
            }
        }

        
        public async Task<IActionResult> GetAllOrdersWithProducts(string include)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {

                    if (include.ToLower() == "product")
                    {
                        cmd.CommandText = @"SELECT o.Id as OrderId, o.CustomerId, o.PaymentTypeId, o.OrderDate,
                                                    op.Id as OrderProductId, 
	                                                p.Id as ProductId, p.ProductName, p.ProductTypeId, p.Price, p.Quantity, p.[Description]
                                            FROM [Order] o LEFT JOIN OrderProduct op ON o.Id = op.OrderId
                                                           LEFT JOIN Product p on p.Id = op.ProductId";
                        SqlDataReader reader = cmd.ExecuteReader();

                        Dictionary<int, Order> orders = new Dictionary<int, Order>();
                        while (reader.Read())
                        {
                            int orderId = reader.GetInt32(reader.GetOrdinal("OrderId"));
                            if (!orders.ContainsKey(orderId))
                            {
                                Order order = new Order
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("OrderId")),
                                    CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                    PaymentTypeId = reader.GetInt32(reader.GetOrdinal("PaymentTypeId")),
                                    OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                                };

                                orders.Add(orderId, order);
                            }
                            Order fromDictionary = orders[orderId];

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
                                fromDictionary.Products.Add(product);
                            }
                        }

                        reader.Close();
                        return Ok(orders.Values);
                    }

                }
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Order order)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // More string interpolation
                    cmd.CommandText = @"
                        INSERT INTO [Order] (CustomerId, PaymentTypeId, OrderDate)
                        OUTPUT INSERTED.Id
                        VALUES (@customerId, @paymentTypeId, @orderDate)
                    ";
                    cmd.Parameters.Add(new SqlParameter("@customerId", order.CustomerId));
                    cmd.Parameters.Add(new SqlParameter("@paymentTypeId", order.PaymentTypeId));
                    cmd.Parameters.Add(new SqlParameter("@orderDate", order.OrderDate));

                    order.Id = (int)await cmd.ExecuteScalarAsync();

                    return CreatedAtRoute("GetOrder", new { id = order.Id }, order);
                }
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Order order)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE [Order]
                                            SET CustomerId = @customerId,
                                                PaymentTypeId = @paymentTypeId,
                                                OrderDate = @orderDate
                                            WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@customerId", order.CustomerId));
                        cmd.Parameters.Add(new SqlParameter("@paymentTypeId", order.PaymentTypeId));
                        cmd.Parameters.Add(new SqlParameter("@orderDate", order.OrderDate));
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        int rowsAffected = cmd.ExecuteNonQuery();
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
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"DELETE FROM OrderProduct WHERE OrderId = @id
                                            DELETE FROM [Order] WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        int rowsAffected = cmd.ExecuteNonQuery();
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
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }


        private bool OrderExists(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id FROM [Order] WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();

                    return reader.Read();
                }
            }
        }
    }
}