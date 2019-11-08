using BangazonAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestBangazonAPI
{
    public class TestOrder
    {
        //Test - get all orders
        [Fact]

        public async Task Test_Get_All_OrdersTypes()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */


                /*
                    ACT
                */
                var response = await client.GetAsync("/api/order");


                string responseBody = await response.Content.ReadAsStringAsync();
                var orders = JsonConvert.DeserializeObject<List<Order>>(responseBody);
                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(orders.Count > 0);
            }
        }

        //---------------------------------------------//
        [Fact]
        public async Task Test_Get_All_Open_Orders()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */


                /*
                    ACT
                */
                var response = await client.GetAsync("/api/order?completed=false");


                string responseBody = await response.Content.ReadAsStringAsync();
                var orders = JsonConvert.DeserializeObject<List<Order>>(responseBody);
                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(orders.Count > 0);
            }
        }

        //---------------------------------------------//
        //GET single order
        [Fact]
        public async Task Test_Get_Order()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */


                /*
                    ACT
                */
                var response = await client.GetAsync("/api/Order/16");


                string responseBody = await response.Content.ReadAsStringAsync();
                var order = JsonConvert.DeserializeObject<Product>(responseBody);
                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(order.Id > 0);
            }
        }

        //////---------------------------------------------//
        //////PUT Editing a order
        [Fact]
        public async Task TestPutOrder()
        {


            using (var client = new APIClientProvider().Client)
            {
                //Put Section

                Order modifiedOrder = new Order
                {
                   
                    OrderDate = DateTime.Now,
                    PaymentTypeId = 12,
                    CustomerId = 1,
                    IsCompleted = false
                    
                };
                var modifiedOrderAsJSON = JsonConvert.SerializeObject(modifiedOrder);

                var response = await client.PutAsync(
                    "api/order/18",
                    new StringContent(modifiedOrderAsJSON, Encoding.UTF8, "application/json"));
                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

                /*
                GET section
                Verify that the PUT operation was successful
                */

                var getOrder = await client.GetAsync("/api/order/18");
                getOrder.EnsureSuccessStatusCode();

                string getOrderBody = await getOrder.Content.ReadAsStringAsync();
                Order newOrder = JsonConvert.DeserializeObject<Order>(getOrderBody);

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
                

            }
        }

        ////POST test for posting a new Order
        [Fact]

        public async Task TestPostNewOrder()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Arrange
                // create a new producttype object to send to the database
                Order newOrder = new Order
                {
                    CustomerId = 3,
                    PaymentTypeId = 3,
                    OrderDate = DateTime.Now,
                    IsCompleted = false


                };
                //Serialize the object into a json string
                var newOrderAsJson = JsonConvert.SerializeObject(newOrder);

                //Act
                //User the client to send the request and store the response
                var response = await client.PostAsync("api/order/",
                    new StringContent(newOrderAsJson, Encoding.UTF8, "application/json"));

                //Store the json body of the response
                string responseBody = await response.Content.ReadAsStringAsync();

                //Deserialize the JSON into an instance of a Order
                var newOrderObject = JsonConvert.DeserializeObject<Order>(responseBody);


                //ASSERT


                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal(3, newOrderObject.PaymentTypeId);


            }
        }

        ////test for deleting a producttype
        [Fact]
        public async Task TestDeleteOrder()
        {
            using (var client = new APIClientProvider().Client)
            {
                int deleteId = 19;

                //Arrange

                //Act
                var response = await client.DeleteAsync($"/api/order/{deleteId}");
                string responseBody = await response.Content.ReadAsStringAsync();
                var order = JsonConvert.DeserializeObject<Order>(responseBody);

                //Assert
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            }
        }
    }
}
