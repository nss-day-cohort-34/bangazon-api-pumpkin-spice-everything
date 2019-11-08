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
    public class TestTrainingPrograms
    {
        //Test - get all productsTypes
        [Fact]

        public async Task Test_Get_All_TrainingPrograms()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */


                /*
                    ACT
                */
                var response = await client.GetAsync("/api/trainingprograms");


                string responseBody = await response.Content.ReadAsStringAsync();
                var productTypes = JsonConvert.DeserializeObject<List<TrainingProgram>>(responseBody);
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
        public async Task Test_Get_TrainingProgram()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */


                /*
                    ACT
                */
                var response = await client.GetAsync("/api/trainingprograms/1");


                string responseBody = await response.Content.ReadAsStringAsync();
                var productType = JsonConvert.DeserializeObject<TrainingProgram>(responseBody);
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
        public async Task TestPutTrainingProgram()
        {


            using (var client = new APIClientProvider().Client)
            {
                //Put Section

                TrainingProgram modifiedTrainingProgram = new TrainingProgram
                {
                    Id = 1,
                    ProgramName = "Jedi Training",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    MaxAttendees = 11
                };
                var modifiedTrainingProgramAsJSON = JsonConvert.SerializeObject(modifiedTrainingProgram);

                var response = await client.PutAsync(
                    "api/trainingprograms/1",
                    new StringContent(modifiedTrainingProgramAsJSON, Encoding.UTF8, "application/json"));
                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

                /*
                GET section
                Verify that the PUT operation was successful
                */

                var getTrainingProgram = await client.GetAsync("/api/trainingprograms/1");
                getTrainingProgram.EnsureSuccessStatusCode();

                string getTrainingProgramBody = await getTrainingProgram.Content.ReadAsStringAsync();
                ProductType newTrainingProgram = JsonConvert.DeserializeObject<ProductType>(getTrainingProgramBody);

                Assert.Equal(HttpStatusCode.OK, getTrainingProgram.StatusCode);

            }
        }

        //POST test for posting a new ProductType
        [Fact]

        public async Task TestPostNewTrainingProgram()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Arrange
                // create a new producttype object to send to the database
                TrainingProgram newTrainingProgram = new TrainingProgram
                {

                    ProgramName = "Jedi Training",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    MaxAttendees = 11

                };
                //Serialize the object into a json string
                var newTrainingProgramAsJson = JsonConvert.SerializeObject(newTrainingProgram);

                //Act
                //User the client to send the request and store the response
                var response = await client.PostAsync("api/trainingprograms",
                    new StringContent(newTrainingProgramAsJson, Encoding.UTF8, "application/json"));

                //Store the json body of the response
                string responseBody = await response.Content.ReadAsStringAsync();

                //Deserialize the JSON into an instance of a ProductType
                var newTrainingProgramObject = JsonConvert.DeserializeObject<TrainingProgram>(responseBody);


                //ASSERT


                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal("Jedi Training", newTrainingProgramObject.ProgramName);


            }
        }

        //test for deleting a producttype
        [Fact]
        public async Task TestDeleteTrainingProgram()
        {
            using (var client = new APIClientProvider().Client)
            {
                int deleteId = 1;

                //Arrange

                //Act
                var response = await client.DeleteAsync($"/api/trainingprograms/{deleteId}");
                string responseBody = await response.Content.ReadAsStringAsync();
                var trainingProgramAsJSON = JsonConvert.DeserializeObject<TrainingProgram>(responseBody);

                //Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            }
        }
    }
}