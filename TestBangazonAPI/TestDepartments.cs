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
    public class TestDepartments
    {
        //Test - get all departments
        [Fact]

        public async Task Test_Get_All_Departments()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */


                /*
                    ACT
                */
                var response = await client.GetAsync("/api/department");


                string responseBody = await response.Content.ReadAsStringAsync();
                var departments = JsonConvert.DeserializeObject<List<Customer>>(responseBody);
                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(departments.Count > 0);
            }
        }

        //GET single department
        [Fact]
        public async Task Test_Get_Department()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */


                /*
                    ACT
                */
                var response = await client.GetAsync("/api/department/2");


                string responseBody = await response.Content.ReadAsStringAsync();
                var department = JsonConvert.DeserializeObject<Customer>(responseBody);
                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(department.Id > 0);
            }
        }

        //PUT Editing a department
        [Fact]
        public async Task TestPutDepartment()
        {
            //New first name to change
            string NewName = "Janitorial Services";

            using (var client = new APIClientProvider().Client)
            {
                //Put Section
                
                Department modifiedDepartment = new Department
                {
                    Id = 1,
                    Name = NewName,
                    Budget = 100,
                    
                };
                var modifiedDepartmentAsJSON = JsonConvert.SerializeObject(modifiedDepartment);

                var response = await client.PutAsync(
                    "api/department/1",
                    new StringContent(modifiedDepartmentAsJSON, Encoding.UTF8, "application/json"));
                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

                /*
                GET section
                Verify that the PUT operation was successful
                */

                var getDepartment = await client.GetAsync("/api/department/1");
                getDepartment.EnsureSuccessStatusCode();

                string getDepartmentBody = await getDepartment.Content.ReadAsStringAsync();
                Department newDepartment = JsonConvert.DeserializeObject<Department>(getDepartmentBody);

                Assert.Equal(HttpStatusCode.OK, getDepartment.StatusCode);
                Assert.Equal(modifiedDepartment.Name, newDepartment.Name);
 
            }
        }

        //POST test for posting a new department
        [Fact]

        public async Task TestPostNewDepartment()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Arrange
                // create a new customer object to send to the database
                Department newDepartment = new Department
                {
                    Name = "Security",
                    Budget = 50000

                };
                //Serialize the object into a json string
                var newDepartmentAsJson = JsonConvert.SerializeObject(newDepartment);

                //Act
                //User the client to send the request and store the response
                var response = await client.PostAsync("api/department",
                    new StringContent(newDepartmentAsJson, Encoding.UTF8, "application/json"));

                //Store the json body of the response
                string responseBody = await response.Content.ReadAsStringAsync();

                //Deserialize the JSON into an instance of a Department
                var newDepartmentObject = JsonConvert.DeserializeObject<Department>(responseBody);


                //ASSERT


                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal("Security", newDepartmentObject.Name);
                Assert.Equal(50000, newDepartmentObject.Budget);


            }
        }
    }
}
