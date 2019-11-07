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
                   
                    OrderDate = DateTime.Today,
                    PaymentTypeId = 3,
                    CustomerId = 1,
                    isCompleted = false
                    
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

                Assert.Equal(HttpStatusCode.OK, getOrder.StatusCode);

            }
        }

        ////POST test for posting a new ProductType
        //[Fact]

        //public async Task TestPostNewProductType()
        //{
        //    using (var client = new APIClientProvider().Client)
        //    {
        //        // Arrange
        //        // create a new producttype object to send to the database
        //        ProductType newProductType = new ProductType
        //        {

        //            TypeName = "Baby",


        //        };
        //        //Serialize the object into a json string
        //        var newProductTypeAsJson = JsonConvert.SerializeObject(newProductType);

        //        //Act
        //        //User the client to send the request and store the response
        //        var response = await client.PostAsync("api/producttypes",
        //            new StringContent(newProductTypeAsJson, Encoding.UTF8, "application/json"));

        //        //Store the json body of the response
        //        string responseBody = await response.Content.ReadAsStringAsync();

        //        //Deserialize the JSON into an instance of a ProductType
        //        var newProductTypeObject = JsonConvert.DeserializeObject<ProductType>(responseBody);


        //        //ASSERT


        //        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        //        Assert.Equal("Baby", newProductTypeObject.TypeName);


        //    }
        //}

        ////test for deleting a producttype
        //[Fact]
        //public async Task TestDeleteProductType()
        //{
        //    using (var client = new APIClientProvider().Client)
        //    {
        //        int deleteId = 12;

        //        //Arrange

        //        //Act
        //        var response = await client.DeleteAsync($"/api/producttypes/{deleteId}");
        //        string responseBody = await response.Content.ReadAsStringAsync();
        //        var student = JsonConvert.DeserializeObject<ProductType>(responseBody);

        //        //Assert
        //        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        //    }
        //}
    }
}
