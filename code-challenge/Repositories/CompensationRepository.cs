using System.Linq;
using System.Threading.Tasks;
using challenge.Data;
using challenge.Models;
using Microsoft.EntityFrameworkCore;

namespace challenge.Repositories
{
    public class CompensationRepository : ICompensationRepository
    {
        private readonly EmployeeContext _employeeContext;

        public CompensationRepository(EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
        }

        public Compensation Add(Compensation compensation)
        {
            compensation.CompensationId = System.Guid.NewGuid();
            _employeeContext.Compensations.Add(compensation);
            return compensation;
        }

        public Compensation GetByEmployeeId(string employeeId)
        {
            // Find the compensation with the latest effective date as the employee's current compensation.
            // Future extensions could allow working with the compensation history.
            return _employeeContext.Compensations
                .Include(c => c.Employee)
                .OrderByDescending(c => c.EffectiveDate)
                .FirstOrDefault(c => c.Employee.EmployeeId == employeeId);
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }
    }
}