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
    public class TestCustomers
    {
        [Fact]
        public async Task Test_Get_All_Customers()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */


                /*
                    ACT
                */
                var response = await client.GetAsync("/api/customers");


                string responseBody = await response.Content.ReadAsStringAsync();
                var customers = JsonConvert.DeserializeObject<List<Customer>>(responseBody);
                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(customers.Count > 0);
            }
        }

        //GET single customer
        [Fact]
        public async Task Test_Get_Customer()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */


                /*
                    ACT
                */
                var response = await client.GetAsync("/api/customers/2");


                string responseBody = await response.Content.ReadAsStringAsync();
                var customer = JsonConvert.DeserializeObject<Customer>(responseBody);
                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(customer.Id > 0);
            }
        }

        //Editing a customer
        [Fact]
        public async Task TestPutCustomer()
        {
            //New first name to change
            string NewFirstName = "Allen";
            
            using (var client = new APIClientProvider().Client)
            {
                //Put Section
                Customer modifiedCustomer = new Customer
                {
                    FirstName = NewFirstName,
                    LastName = "Collins",
                    CreationDate = DateTime.Now,
                    LastActiveDate = DateTime.Now,
                    IsActive = true
                };
                var modifiedCustomerAsJSON = JsonConvert.SerializeObject(modifiedCustomer);

                var response = await client.PutAsync(
                    "api/customers/1",
                    new StringContent(modifiedCustomerAsJSON, Encoding.UTF8, "application/json"));
                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                /*
                GET section
                Verify that the PUT operation was successful
                */

                var getCustomer = await client.GetAsync("/api/customers/1");
                getCustomer.EnsureSuccessStatusCode();

                string getCustomerBody = await getCustomer.Content.ReadAsStringAsync();
                Customer newCustomer = JsonConvert.DeserializeObject<Customer>(getCustomerBody);

                Assert.Equal(HttpStatusCode.OK, getCustomer.StatusCode);
                Assert.Equal(NewFirstName, newCustomer.FirstName);
            }
        }

        //POST test for posting a new student
        [Fact]

        public async Task TestPostNewCustomer()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Arrange
                // create a new customer object to send to the database
                Customer newCustomer = new Customer
                {
                    FirstName = "Troy",
                    LastName = "McClure",
                    CreationDate = DateTime.Now,
                    LastActiveDate = DateTime.Now,
                    IsActive = true

                };
                //Serialize the object into a json string
                var newCustomerAsJson = JsonConvert.SerializeObject(newCustomer);

                //Act
                //User the client to send the request and store the response
                var response = await client.PostAsync("api/customers",
                    new StringContent(newCustomerAsJson, Encoding.UTF8, "application/json"));

                //Store the json body of the response
                string responseBody = await response.Content.ReadAsStringAsync();

                //Deserialize the JSON into an instance of a Customer
                var newCustomerObject = JsonConvert.DeserializeObject<Customer>(responseBody);


                //ASSERT


                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal("Troy", newCustomerObject.FirstName);
                Assert.Equal("McClure", newCustomerObject.LastName);
               

            }
        }
    }
}
