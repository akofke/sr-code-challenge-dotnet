using challenge.Models;

namespace challenge.Services
{
    public interface IReportingStructureService
    {
        ReportingStructure GetByEmployeeId(string employeeId);
    }
}