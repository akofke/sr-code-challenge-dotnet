using challenge.Models;
using challenge.Repositories;

namespace challenge.Services
{
    public class CompensationService : ICompensationService
    {
        private readonly ICompensationRepository _compensationRepository;
        private readonly IEmployeeService _employeeService;

        public CompensationService(ICompensationRepository compensationRepository, IEmployeeService employeeService)
        {
            _compensationRepository = compensationRepository;
            _employeeService = employeeService;
        }

        public Compensation GetByEmployeeId(string employeeId)
        {
            if (string.IsNullOrEmpty(employeeId))
            {
                return null;
            }

            return _compensationRepository.GetByEmployeeId(employeeId);
        }

        public Compensation CreateForEmployeeId(string employeeId, Compensation compensation)
        {
            var employee = _employeeService.GetById(employeeId);
            if (employee == null || compensation == null)
            {
                return null;
            }

            compensation.Employee = employee;
            _compensationRepository.Add(compensation);
            _compensationRepository.SaveAsync().Wait();
            return compensation;
        }
    }
}