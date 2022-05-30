using challenge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace challenge.Controllers
{
    [Route("api/reporting-structure")]
    public class ReportingStructureController : Controller
    {
        private readonly ILogger<ReportingStructureController> _logger;
        private readonly IReportingStructureService _reportingStructureService;

        public ReportingStructureController(ILogger<ReportingStructureController> logger, IReportingStructureService reportingStructureService)
        {
            _logger = logger;
            _reportingStructureService = reportingStructureService;
        }

        [HttpGet("{id}", Name = "getReportingStructureByEmployeeId")]
        public IActionResult GetReportingStructureByEmployeeId(string id)
        {
            _logger.LogDebug("Received reporting structure get request for {}", id);

            var reportingStructure = _reportingStructureService.GetByEmployeeId(id);

            if (reportingStructure == null)
            {
                return NotFound();
            }

            return Ok(reportingStructure);
        }
        
    }
}