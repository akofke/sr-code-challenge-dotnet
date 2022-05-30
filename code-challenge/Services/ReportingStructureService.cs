using challenge.Models;
using challenge.Repositories;

namespace challenge.Services
{
    public class ReportingStructureService : IReportingStructureService
    {
        private readonly IReportingStructureRepository _reportingStructureRepository;
        private readonly IEmployeeService _employeeService;

        public ReportingStructureService(IReportingStructureRepository reportingStructureRepository, IEmployeeService employeeService)
        {
            _reportingStructureRepository = reportingStructureRepository;
            _employeeService = employeeService;
        }

        public ReportingStructure GetByEmployeeId(string employeeId)
        {
            var employee = _employeeService.GetById(employeeId);
            if (employee == null)
            {
                return null;
            }

            return _reportingStructureRepository.Get(employee);
        }
    }
}