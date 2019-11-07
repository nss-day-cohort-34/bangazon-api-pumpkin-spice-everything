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
    public class TestProductsTypes
    {
        //Test - get all productsTypes
        [Fact]

        public async Task Test_Get_All_ProductsTypes()
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
                var productTypes = JsonConvert.DeserializeObject<List<ProductType>>(responseBody);
                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(productTypes.Count > 0);
            }
        }
        //---------------------------------------------//
        //GET single product
        [Fact]
        public async Task Test_Get_ProductTypes()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */


                /*
                    ACT
                */
                var response = await client.GetAsync("/api/ProductTypes/1");


                string responseBody = await response.Content.ReadAsStringAsync();
                var productType = JsonConvert.DeserializeObject<Product>(responseBody);
                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(productType.Id > 0);
            }
        }

        ////---------------------------------------------//
        ////PUT Editing a producttypes
        [Fact]
        public async Task TestPutProductType()
        {


            using (var client = new APIClientProvider().Client)
            {
                //Put Section

                ProductType modifiedProductType = new ProductType
                {
                    Id = 1,
                    TypeName = "Home Electronics",

                };
                var modifiedProductTypeAsJSON = JsonConvert.SerializeObject(modifiedProductType);

                var response = await client.PutAsync(
                    "api/producttypes/1",
                    new StringContent(modifiedProductTypeAsJSON, Encoding.UTF8, "application/json"));
                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

                /*
                GET section
                Verify that the PUT operation was successful
                */

                var getProductType = await client.GetAsync("/api/producttypes/1");
                getProductType.EnsureSuccessStatusCode();

                string getProductTypeBody = await getProductType.Content.ReadAsStringAsync();
                ProductType newProductType = JsonConvert.DeserializeObject<ProductType>(getProductTypeBody);

                Assert.Equal(HttpStatusCode.OK, getProductType.StatusCode);

            }
        }

        //POST test for posting a new ProductType
        [Fact]

        public async Task TestPostNewProductType()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Arrange
                // create a new producttype object to send to the database
                ProductType newProductType = new ProductType
                {

                    TypeName = "Baby",


                };
                //Serialize the object into a json string
                var newProductTypeAsJson = JsonConvert.SerializeObject(newProductType);

                //Act
                //User the client to send the request and store the response
                var response = await client.PostAsync("api/producttypes",
                    new StringContent(newProductTypeAsJson, Encoding.UTF8, "application/json"));

                //Store the json body of the response
                string responseBody = await response.Content.ReadAsStringAsync();

                //Deserialize the JSON into an instance of a ProductType
                var newProductTypeObject = JsonConvert.DeserializeObject<ProductType>(responseBody);


                //ASSERT


                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal("Baby", newProductTypeObject.TypeName);


            }
        }

        //test for deleting a producttype
        [Fact]
        public async Task TestDeleteProductType()
        {
            using (var client = new APIClientProvider().Client)
            {
                int deleteId = 12;

                //Arrange

                //Act
                var response = await client.DeleteAsync($"/api/producttypes/{deleteId}");
                string responseBody = await response.Content.ReadAsStringAsync();
                var student = JsonConvert.DeserializeObject<ProductType>(responseBody);

                //Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            }
        }
    }
}
