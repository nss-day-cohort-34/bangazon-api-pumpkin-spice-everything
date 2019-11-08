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
    public class TestProducts
    {
        //Test - get all products
        [Fact]

        public async Task Test_Get_All_Products()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */


                /*
                    ACT
                */
                var response = await client.GetAsync("/api/products");


                string responseBody = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<List<Product>>(responseBody);
                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(products.Count > 0);
            }
        }
        //---------------------------------------------//
        //GET single product
        [Fact]
        public async Task Test_Get_Product()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */


                /*
                    ACT
                */
                var response = await client.GetAsync("/api/products/3");


                string responseBody = await response.Content.ReadAsStringAsync();
                var product = JsonConvert.DeserializeObject<Product>(responseBody);
                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(product.Id > 0);
            }
        }

        //---------------------------------------------//
        //PUT Editing a product
        [Fact]
        public async Task TestPutProduct()
        {
           

            using (var client = new APIClientProvider().Client)
            {
                //Put Section

                Product modifiedProduct = new Product
                {
                    Id = 3,
                    ProductName = "iPadPro",
                    Price = 600,
                    Description = "An overpriced tablet",
                    Quantity = 12,
                    ProductTypeId = 1,
                    CustomerId = 5

                };
                var modifiedProductAsJSON = JsonConvert.SerializeObject(modifiedProduct);

                var response = await client.PutAsync(
                    "api/products/3",
                    new StringContent(modifiedProductAsJSON, Encoding.UTF8, "application/json"));
                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

                /*
                GET section
                Verify that the PUT operation was successful
                */

                var getProduct = await client.GetAsync("/api/products/3");
                getProduct.EnsureSuccessStatusCode();

                string getProductBody = await getProduct.Content.ReadAsStringAsync();
                Product newProduct = JsonConvert.DeserializeObject<Product>(getProductBody);

                Assert.Equal(HttpStatusCode.OK, getProduct.StatusCode);

            }
        }

        //POST test for posting a new Product
        [Fact]

        public async Task TestPostNewProduct()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Arrange
                // create a new customer object to send to the database
                Product newProduct = new Product
                {
                    
                    ProductName = "Bag of glass",
                    Price = 20,
                    Description = "It's a bag of glass, fun for kids",
                    Quantity = 24,
                    ProductTypeId = 2,
                    CustomerId = 3

                };
                //Serialize the object into a json string
                var newProductAsJson = JsonConvert.SerializeObject(newProduct);

                //Act
                //User the client to send the request and store the response
                var response = await client.PostAsync("api/products",
                    new StringContent(newProductAsJson, Encoding.UTF8, "application/json"));

                //Store the json body of the response
                string responseBody = await response.Content.ReadAsStringAsync();

                //Deserialize the JSON into an instance of a Product
                var newProductObject = JsonConvert.DeserializeObject<Product>(responseBody);


                //ASSERT


                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal("Bag of glass", newProductObject.ProductName);
                Assert.Equal(20, newProductObject.Price);


            }
        }

        //test for deleting a product
        [Fact]
        public async Task TestDeleteProduct()
        {
            using (var client = new APIClientProvider().Client)
            {
                int deleteId = 4;

                //Arrange

                //Act
                var response = await client.DeleteAsync($"/api/products/{deleteId}");
                string responseBody = await response.Content.ReadAsStringAsync();
                var student = JsonConvert.DeserializeObject<Product>(responseBody);

                //Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            }
        }
    }
}
