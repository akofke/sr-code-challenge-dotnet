using challenge.Models;
using System.Threading.Tasks;

namespace challenge.Repositories
{
    public interface ICompensationRepository
    {
        Compensation Add(Compensation compensation);
        Compensation GetByEmployeeId(string employeeId);

        Task SaveAsync();
    }
}