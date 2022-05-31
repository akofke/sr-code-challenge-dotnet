using challenge.Models;

namespace challenge.Services
{
    public interface ICompensationService
    {
        Compensation CreateForEmployeeId(string employeeId, Compensation compensation);
        Compensation GetByEmployeeId(string employeeId);
    }
}