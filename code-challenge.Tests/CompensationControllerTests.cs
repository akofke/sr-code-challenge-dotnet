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
        private HttpClient _httpClient;
        private TestServer _testServer;

        [TestInitialize]
        public void SetUp()
        {
            // Need to re-do setup for each test method to start with a clean database
            _testServer = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<TestServerStartup>()
                .UseEnvironment("Development"));

            _httpClient = _testServer.CreateClient();
        }

        [TestCleanup]
        public void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }


        private Task<HttpResponseMessage> CreateTestCompensation(string employeeId, Compensation compensation)
        {
            var requestContent = new JsonSerialization().ToJson(compensation);

            return _httpClient.PostAsync($"api/compensation/{employeeId}",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
        }

        [TestMethod]
        public async Task CreateCompensation_Returns_Created()
        {
            const string employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var compensation = new Compensation()
            {
                Salary = new decimal(100000.40),
                EffectiveDate = new DateTime(2022, 5, 30)
            };
            var response = await CreateTestCompensation(employeeId, compensation);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            var newCompensation = response.DeserializeContent<Compensation>();
            Assert.AreEqual(employeeId, newCompensation.Employee.EmployeeId);
            Assert.AreNotEqual(System.Guid.Empty, newCompensation.Salary);
            Assert.AreEqual(compensation.Salary, newCompensation.Salary);
            Assert.AreEqual(compensation.EffectiveDate, newCompensation.EffectiveDate);
        }

        [TestMethod]
        public async Task GetCompensationAfterCreate_Returns_Ok()
        {
            const string employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var compensation = new Compensation()
            {
                Salary = new decimal(100000.40),
                EffectiveDate = new DateTime(2022, 5, 30)
            };

            var createResponse = await CreateTestCompensation(employeeId, compensation);

            Assert.AreEqual(HttpStatusCode.Created, createResponse.StatusCode);


            var response = await _httpClient.GetAsync($"api/compensation/{employeeId}");

            var retrievedCompensation = response.DeserializeContent<Compensation>();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(employeeId, retrievedCompensation.Employee.EmployeeId);
            Assert.AreEqual(compensation.Salary, retrievedCompensation.Salary);
            Assert.AreEqual(compensation.EffectiveDate, retrievedCompensation.EffectiveDate);
        }

        [TestMethod]
        public async Task CreateSecondCompensation_ReplacesFirst()
        {
            const string employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var firstCompensation = new Compensation()
            {
                Salary = new decimal(100000.40),
                EffectiveDate = new DateTime(2022, 5, 30)
            };

            var firstCreateResponse = await CreateTestCompensation(employeeId, firstCompensation);

            Assert.AreEqual(HttpStatusCode.Created, firstCreateResponse.StatusCode);

            var secondCompensation = new Compensation()
            {
                Salary = new decimal(100001.40),
                EffectiveDate = new DateTime(2022, 5, 31)
            };

            var secondCreateResponse = await CreateTestCompensation(employeeId, secondCompensation);
            var newCompensation = secondCreateResponse.DeserializeContent<Compensation>();

            Assert.AreEqual(HttpStatusCode.Created, secondCreateResponse.StatusCode);
            Assert.AreEqual(secondCompensation.Salary, newCompensation.Salary);
            Assert.AreEqual(secondCompensation.EffectiveDate, newCompensation.EffectiveDate);

            // Test GET to make sure there's no exceptions from duplicate Compensation
            var response = await _httpClient.GetAsync($"api/compensation/{employeeId}");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var retrievedCompensation = response.DeserializeContent<Compensation>();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(employeeId, retrievedCompensation.Employee.EmployeeId);
            Assert.AreEqual(secondCompensation.Salary, retrievedCompensation.Salary);
            Assert.AreEqual(secondCompensation.EffectiveDate, retrievedCompensation.EffectiveDate);
        }


    }
}
