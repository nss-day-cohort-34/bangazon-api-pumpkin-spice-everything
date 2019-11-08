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
        public class TestEmployees
        {
        //Test - get all employees
        [Fact]
            public async Task Test_Get_All_Employees()
            {
                using (var client = new APIClientProvider().Client)
                {
                    /*
                        ARRANGE
                    */


                    /*
                        ACT
                    */
                    var response = await client.GetAsync("/api/employee");


                    string responseBody = await response.Content.ReadAsStringAsync();
                    var employees = JsonConvert.DeserializeObject<List<Employee>>(responseBody);
                    /*
                        ASSERT
                    */
                    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                    Assert.True(employees.Count > 0);
                }
            }
        //GET single employee
        [Fact]
        public async Task Test_Get_Employee()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */


                /*
                    ACT
                */
                var response = await client.GetAsync("/api/employee/1");


                string responseBody = await response.Content.ReadAsStringAsync();
                var employee = JsonConvert.DeserializeObject<Employee>(responseBody);
                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(employee.Id > 0);
            }
        }

        //POST test for posting a new employee
        [Fact]

        public async Task TestPostNewEmployee()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Arrange
                // create a new employee object to send to the database
                Employee newEmployee = new Employee
                {
                    FirstName = "Lina",
                    LastName = "Patton",
                    DepartmentId = 4,
                    IsSupervisor = true,
                    StartDate = new DateTime(2019, 11, 05)
                };
                //Serialize the object into a json string
                var newEmployeeAsJson = JsonConvert.SerializeObject(newEmployee);

                //Act
                //User the client to send the request and store the response
                var response = await client.PostAsync("api/employee",
                    new StringContent(newEmployeeAsJson, Encoding.UTF8, "application/json"));

                //Store the json body of the response
                string responseBody = await response.Content.ReadAsStringAsync();

                //Deserialize the JSON into an instance of an Employee
                var newEmployeeObject = JsonConvert.DeserializeObject<Employee>(responseBody);
                //try to abstract dateTime now???
                //var dateTimeNow = DateTime.Now;

                //ASSERT


                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal("Lina", newEmployeeObject.FirstName);
                Assert.Equal("Patton", newEmployeeObject.LastName);
                Assert.Equal(4, newEmployeeObject.DepartmentId);
                //Assert.True(true, newEmployeeObject.IsSupervisor);
                //Assert.(new DateTime(2019,11,05), newEmployeeObject.StartDate);


            }
        }


        //PUT Editing an Emplpoyee
        [Fact]
        public async Task Test_Put_Employee()
        {
            //New first name to change
            string NewFirstName = "Hudson Derpy";

            using (var client = new APIClientProvider().Client)
            {
                //Put Section

                Employee modifiedEmployee = new Employee
                {
                    Id = 1,
                    FirstName = NewFirstName,
                    LastName = "Patton",
                    DepartmentId = 4,
                    IsSupervisor = true,
                    StartDate = DateTime.Now,

                };
                var modifiedEmployeeAsJSON = JsonConvert.SerializeObject(modifiedEmployee);

                var response = await client.PutAsync(
                    "api/employee/6",
                    new StringContent(modifiedEmployeeAsJSON, Encoding.UTF8, "application/json"));
                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

                /*
                GET section
                Verify that the PUT operation was successful
                */

                var getEmployee = await client.GetAsync("/api/employee/6");
                getEmployee.EnsureSuccessStatusCode();

                string getEmployeeBody = await getEmployee.Content.ReadAsStringAsync();
                Employee newEmployee = JsonConvert.DeserializeObject<Employee>(getEmployeeBody);

                Assert.Equal(HttpStatusCode.OK, getEmployee.StatusCode);
                Assert.Equal(NewFirstName, newEmployee.FirstName);

            }
        }
    }
}
