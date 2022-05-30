using challenge.Models;

namespace challenge.Repositories
{
    public interface IReportingStructureRepository
    {
        ReportingStructure Get(Employee employee); 
    }
}