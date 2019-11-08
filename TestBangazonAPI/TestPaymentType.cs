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
    public class TestPaymentType
    {
        //Test - get all paymentTypes
        [Fact]

        public async Task Test_Get_All_PaymentTypes()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */


                /*
                    ACT
                */
                var response = await client.GetAsync("/api/paymenttype");


                string responseBody = await response.Content.ReadAsStringAsync();
                var paymentTypes = JsonConvert.DeserializeObject<List<PaymentType>>(responseBody);
                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(paymentTypes.Count > 0);
            }
        }
        //---------------------------------------------//
        //GET single product
        [Fact]
        public async Task Test_Get_PaymentType()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */


                /*
                    ACT
                */
                var response = await client.GetAsync("/api/paymenttype/2");


                string responseBody = await response.Content.ReadAsStringAsync();
                var paymentType = JsonConvert.DeserializeObject<PaymentType>(responseBody);
                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(paymentType.Id > 0);
            }
        }

        //////---------------------------------------------//
        //////PUT Editing a PaymentTypes
        [Fact]
        public async Task TestPutPaymentType()
        {


            using (var client = new APIClientProvider().Client)
            {
                //Put Section

                PaymentType modifiedPaymentType = new PaymentType
                {
                    Id = 2,
                    Type = "PayPal",
                    AcctNumber = "12345600",
                    CustomerId = 3,
                };
                var modifiedPaymentTypeAsJSON = JsonConvert.SerializeObject(modifiedPaymentType);

                var response = await client.PutAsync(
                    "api/paymenttype/2",
                    new StringContent(modifiedPaymentTypeAsJSON, Encoding.UTF8, "application/json"));
                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

                /*
                GET section
                Verify that the PUT operation was successful
                */

                var getPaymentType = await client.GetAsync("/api/paymenttype/2");
                getPaymentType.EnsureSuccessStatusCode();

                string getPaymentTypeBody = await getPaymentType.Content.ReadAsStringAsync();
                PaymentType newPaymentType = JsonConvert.DeserializeObject<PaymentType>(getPaymentTypeBody);

                Assert.Equal(HttpStatusCode.OK, getPaymentType.StatusCode);

            }
        }

        //POST test for posting a new ProductType
        [Fact]

        public async Task TestPostNewPaymentType()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Arrange
                // create a new paymenttype object to send to the database
                PaymentType newPaymentType = new PaymentType
                {
                    Type = "Google Pay",
                    AcctNumber = "9876543421",
                    CustomerId = 2,



                };
                //Serialize the object into a json string
                var newPaymentTypeAsJson = JsonConvert.SerializeObject(newPaymentType);

                //Act
                //User the client to send the request and store the response
                var response = await client.PostAsync("api/paymenttype",
                    new StringContent(newPaymentTypeAsJson, Encoding.UTF8, "application/json"));

                //Store the json body of the response
                string responseBody = await response.Content.ReadAsStringAsync();

                //Deserialize the JSON into an instance of a PaymentType
                var newPaymentTypeObject = JsonConvert.DeserializeObject<PaymentType>(responseBody);


                //ASSERT


                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal("Google Pay", newPaymentTypeObject.Type);


            }
        }

        ////test for deleting a PaymentType
        [Fact]
        public async Task TestDeletePaymentType()
        {
            using (var client = new APIClientProvider().Client)
            {
                int deleteId = 2;

                //Arrange

                //Act
                var response = await client.DeleteAsync($"/api/paymenttype/{deleteId}");
                string responseBody = await response.Content.ReadAsStringAsync();
                var student = JsonConvert.DeserializeObject<ProductType>(responseBody);

                //Assert
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            }
        }
    }
}
