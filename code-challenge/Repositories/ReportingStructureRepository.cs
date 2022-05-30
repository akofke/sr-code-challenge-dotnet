using System.Linq;
using challenge.Data;
using challenge.Models;

namespace challenge.Repositories
{
    public class ReportingStructureRepository : IReportingStructureRepository
    {
        private readonly EmployeeContext _employeeContext;

        public ReportingStructureRepository(EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
        }
        

        public ReportingStructure Get(Employee employee)
        {
            var numberOfReports = GetNumberOfReports(employee);
            return new ReportingStructure()
            {
                Employee = employee,
                NumberOfReports = numberOfReports
            };
        }
        
        private int GetNumberOfReports(Employee employee)
        {
            // Ensure that the DirectReports navigation property is loaded.
            //
            // This isn't very efficient if using an external database with a deep employee hierarchy.
            // A more scalable solution would be computing the count in a stored procedure with a recursive CTE,
            // to avoid making round trips to the db for each level. Something like:
            /*
            WITH cte_reports(Id) AS (
                SELECT o.ReportId FROM Employees e INNER JOIN Org o ON e.Id = o.ManagerId where e.Id = @employeeId
                UNION ALL
                select o.ReportId from cte_reports e INNER JOIN Org o ON e.Id = o.ManagerId
            )
            SELECT COUNT(*) FROM cte_reports
            */
            _employeeContext.Entry(employee)
                .Collection(e => e.DirectReports)
                .Load();

            // The number of employees in the DirectReports list, or 0 if it is null
            var directReportsCount = employee.DirectReports?.Count ?? 0;

            // Recursively sum the number of reports for each DirectReport.
            // This ensures that the navigation property is loaded for any arbitrary level of employees.
            var subReportsCount = employee.DirectReports?.Sum(GetNumberOfReports) ?? 0;
            
            return directReportsCount + subReportsCount;
        }
    }
}
