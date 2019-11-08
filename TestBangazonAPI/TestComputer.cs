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
    public class TestComputer
    {
        //Test - get all paymentTypes
        [Fact]
        public async Task Test_Get_All_Computers()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */


                /*
                    ACT
                */
                var response = await client.GetAsync("/api/computer");


                string responseBody = await response.Content.ReadAsStringAsync();
                var computer = JsonConvert.DeserializeObject<List<Computer>>(responseBody);
                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(computer.Count > 0);
            }
        }
        //---------------------------------------------//
        //GET single product
        [Fact]
        public async Task Test_Get_Computer()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */


                /*
                    ACT
                */
                var response = await client.GetAsync("/api/computer/6");


                string responseBody = await response.Content.ReadAsStringAsync();
                var computer = JsonConvert.DeserializeObject<Computer>(responseBody);
                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(computer.Id > 0);
            }
        }

        //////---------------------------------------------//
        //////PUT Editing a PaymentTypes
        [Fact]
        public async Task TestPutComputer()
        {


            using (var client = new APIClientProvider().Client)
            {
                //Put Section

                Computer modifiedComputer = new Computer
                {
                    Id = 6,
                    Make = "MainFrame",
                    Manufacturer = "IBM"
                };
                var modifiedComputerAsJSON = JsonConvert.SerializeObject(modifiedComputer);

                var response = await client.PutAsync(
                    "api/computer/6",
                    new StringContent(modifiedComputerAsJSON, Encoding.UTF8, "application/json"));
                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

                /*
                GET section
                Verify that the PUT operation was successful
                */

                var getComputer = await client.GetAsync("/api/computer/6");
                getComputer.EnsureSuccessStatusCode();

                string getComputerBody = await getComputer.Content.ReadAsStringAsync();
                Computer newComputer = JsonConvert.DeserializeObject<Computer>(getComputerBody);

                Assert.Equal(HttpStatusCode.OK, getComputer.StatusCode);

            }
        }

        //POST test for posting a new Computer
        [Fact]

        public async Task TestPostNewComputer()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Arrange
                // create a new paymenttype object to send to the database
                Computer newComputer = new Computer
                {
                    PurchaseDate = DateTime.Now,
                    Make = "MainFrame",
                    Manufacturer = "IBM"
                };
                //Serialize the object into a json string
                var newComputerAsJson = JsonConvert.SerializeObject(newComputer);

                //Act
                //User the client to send the request and store the response
                var response = await client.PostAsync("api/computer",
                    new StringContent(newComputerAsJson, Encoding.UTF8, "application/json"));

                //Store the json body of the response
                string responseBody = await response.Content.ReadAsStringAsync();

                //Deserialize the JSON into an instance of a PaymentType
                var newComputerObject = JsonConvert.DeserializeObject<Computer>(responseBody);


                //ASSERT


                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal("Main Frame", newComputerObject.Make);


            }
        }

        ////test for deleting a Computer
        [Fact]
        public async Task TestDeleteComputer()
        {
            using (var client = new APIClientProvider().Client)
            {
                int deleteId = 8;

                //Arrange

                //Act
                var response = await client.DeleteAsync($"/api/computer/{deleteId}");
                string responseBody = await response.Content.ReadAsStringAsync();
                var student = JsonConvert.DeserializeObject<Computer>(responseBody);

                //Assert
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            }
        }
    }




}

