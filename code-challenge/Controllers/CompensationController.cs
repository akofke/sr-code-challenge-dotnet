using challenge.Models;
using challenge.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace challenge.Controllers
{
    [Route("api/[controller]")]
    public class CompensationController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ICompensationService _compensationService;
        public CompensationController(ILogger<CompensationController> logger, ICompensationService compensationService)
        {
            _logger = logger;
            _compensationService = compensationService;
        }

        [HttpGet("{employeeId}", Name = "getCompensationByEmployeeId")]
        public IActionResult GetByEmployeeId(string employeeId)
        {
            _logger.LogDebug("Received compensation get request for employee {}", employeeId);

            var compensation = _compensationService.GetByEmployeeId(employeeId);

            if (compensation == null)
            {
                return NotFound();
            }

            return Ok(compensation);
        }

        [HttpPost("{employeeId}")]
        public IActionResult Create(string employeeId, [FromBody] Compensation compensation)
        {
            _logger.LogDebug("Received compensation create request for employee id {} with {}", employeeId, compensation);

            var newCompensation = _compensationService.CreateForEmployeeId(employeeId, compensation);

            return CreatedAtRoute("getCompensationByEmployeeId", new { employeeId }, compensation);
        }
    }
}
