using challenge.Controllers;
using challenge.Data;
using challenge.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using code_challenge.Tests.Integration.Extensions;

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using code_challenge.Tests.Integration.Helpers;
using System.Text;
using System.Threading.Tasks;

namespace code_challenge.Tests.Integration
{
    [TestClass]
    public class CompensationControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<TestServerStartup>()
                .UseEnvironment("Development"));

            _httpClient = _testServer.CreateClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [TestMethod]
        public async Task CreateCompensation_Returns_Created()
        {
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var compensation = new Compensation()
            {
                Salary = new decimal(100000.40),
                EffectiveDate = new DateTime(2022, 5, 30)
            };

            var requestContent = new JsonSerialization().ToJson(compensation);

            var response = await _httpClient.PostAsync($"api/compensation/{employeeId}",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var newCompensation = response.DeserializeContent<Compensation>();
            Assert.AreEqual(employeeId, newCompensation.Employee.EmployeeId);
            Assert.AreEqual(compensation.Salary, newCompensation.Salary);
            Assert.AreEqual(compensation.EffectiveDate, newCompensation.EffectiveDate);
        }

        [TestMethod]
        public async Task GetCompensationAfterCreate_Returns_Ok()
        {
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var compensation = new Compensation()
            {
                Salary = new decimal(100000.40),
                EffectiveDate = new DateTime(2022, 5, 30)
            };

            var requestContent = new JsonSerialization().ToJson(compensation);

            var createResponse = await _httpClient.PostAsync($"api/compensation/{employeeId}",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));

            Assert.AreEqual(HttpStatusCode.Created, createResponse.StatusCode);


            var response = await _httpClient.GetAsync($"api/compensation/{employeeId}");

            var retrievedCompensation = response.DeserializeContent<Compensation>();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(employeeId, retrievedCompensation.Employee.EmployeeId);
            Assert.AreEqual(compensation.Salary, retrievedCompensation.Salary);
            Assert.AreEqual(compensation.EffectiveDate, retrievedCompensation.EffectiveDate);
        }


    }
}
