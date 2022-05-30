using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using challenge.Models;
using code_challenge.Tests.Integration.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace code_challenge.Tests.Integration
{
    [TestClass]
    public class ReportingStructureControllerTests
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
        public async Task GetEmployeeReportingStructure_Returns_Ok()
        {
            // John Lennon employee ID
            const string employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            const int expectedNumberOfReports = 4;

            var response = await _httpClient.GetAsync($"api/reporting-structure/{employeeId}");
            
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var reportingStructure = response.DeserializeContent<ReportingStructure>();
            Assert.AreEqual(employeeId, reportingStructure.Employee.EmployeeId);
            Assert.AreEqual(expectedNumberOfReports, reportingStructure.NumberOfReports);
        }

        [TestMethod]
        public async Task GetEmployeeReportingStructure_ReturnsZeroReports()
        {
            // Paul McCartney employee ID
            const string employeeId = "b7839309-3348-463b-a7e3-5de1c168beb3";
            const int expectedNumberOfReports = 0;

            var response = await _httpClient.GetAsync($"api/reporting-structure/{employeeId}");
            
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var reportingStructure = response.DeserializeContent<ReportingStructure>();
            Assert.AreEqual(employeeId, reportingStructure.Employee.EmployeeId);
            Assert.AreEqual(expectedNumberOfReports, reportingStructure.NumberOfReports);
            
        }
    }
}