using System.Collections.Generic;
using System.Threading.Tasks;
using HRPlanning.Dto;

namespace HRPlanning.Repository
{
    public interface IEmployeeRepository
    {
        Task<List<EmployeeDto>> GetAllAsync();
        Task<EmployeeDetailDto?> GetByIdAsync(int id);
        Task<EmployeeDetailDto> CreateAsync(EmployeeCreateDto dto);
        Task<EmployeeDetailDto?> UpdateAsync(int id, EmployeeUpdateDto dto);
        Task<bool> SoftDeleteAsync(int id);
        Task<MonthlySalaryDto?> AddMonthlySalaryAsync(int employeeId, MonthlySalaryCreateDto dto);
        Task<List<EmployeeDto>> SearchAsync(EmployeeSearchCriteria criteria);
    }
}
