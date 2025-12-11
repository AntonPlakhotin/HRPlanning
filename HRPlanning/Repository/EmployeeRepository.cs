using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRPlanning.Data;
using HRPlanning.Dto;
using HRPlanning.Models;
using Microsoft.EntityFrameworkCore;

namespace HRPlanning.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<EmployeeDto>> GetAllAsync()
        {
            var query = _context.Employees
                .AsNoTracking()
                .Where(e => !e.IsDeleted)
                .Include(e => e.Grade)
                .Include(e => e.Position)
                .OrderBy(e => e.Id);

            // Сначала загружаем сущности асинхронно, затем маппим в DTO в памяти
            var entities = await query.ToListAsync();
            var items = entities.Select(MapToEmployeeDto).ToList();

            return items;
        }

        public async Task<EmployeeDetailDto?> GetByIdAsync(int id)
        {
            var employee = await _context.Employees
                .AsNoTracking()
                .Include(e => e.Grade)
                .Include(e => e.Position)
                .Include(e => e.MonthlySalaries)
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

            return employee is null ? null : MapToEmployeeDetailDto(employee);
        }

        public async Task<EmployeeDetailDto> CreateAsync(EmployeeCreateDto dto)
        {
            var employee = new Employee
            {
                FullName = dto.FullName.Trim(),
                HireDate = dto.HireDate,
                GradeId = dto.GradeId,
                PositionId = dto.PositionId,
                Notes = string.IsNullOrWhiteSpace(dto.Notes) ? null : dto.Notes.Trim()
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            var created = await _context.Employees
                .AsNoTracking()
                .Include(e => e.Grade)
                .Include(e => e.Position)
                .Include(e => e.MonthlySalaries)
                .FirstAsync(e => e.Id == employee.Id);

            return MapToEmployeeDetailDto(created);
        }

        public async Task<EmployeeDetailDto?> UpdateAsync(int id, EmployeeUpdateDto dto)
        {
            var employee = await _context.Employees
                .Include(e => e.MonthlySalaries)
                .Include(e => e.Grade)
                .Include(e => e.Position)
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

            if (employee is null)
            {
                return null;
            }

            employee.FullName = dto.FullName?.Trim() ?? employee.FullName;
            employee.HireDate = dto.HireDate ?? employee.HireDate;
            employee.GradeId = dto.GradeId;
            employee.PositionId = dto.PositionId;
            employee.Notes = dto.Notes == null
                ? employee.Notes
                : (string.IsNullOrWhiteSpace(dto.Notes) ? null : dto.Notes.Trim());

            await _context.SaveChangesAsync();

            var updated = await _context.Employees
                .AsNoTracking()
                .Include(e => e.Grade)
                .Include(e => e.Position)
                .Include(e => e.MonthlySalaries)
                .FirstAsync(e => e.Id == employee.Id);

            return MapToEmployeeDetailDto(updated);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

            if (employee is null)
            {
                return false;
            }

            employee.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<MonthlySalaryDto?> AddMonthlySalaryAsync(int employeeId, MonthlySalaryCreateDto dto)
        {
            var employeeExists = await _context.Employees
                .AnyAsync(e => e.Id == employeeId && !e.IsDeleted);

            if (!employeeExists)
            {
                return null;
            }

            var monthlySalary = new MonthlySalary
            {
                EmployeeId = employeeId,
                SalaryDate = dto.SalaryDate,
                Amount = dto.Amount,
                CreatedAt = DateTime.UtcNow
            };

            _context.MonthlySalaries.Add(monthlySalary);
            await _context.SaveChangesAsync();

            return new MonthlySalaryDto
            {
                Id = monthlySalary.Id,
                SalaryDate = monthlySalary.SalaryDate,
                Amount = monthlySalary.Amount,
                CreatedAt = monthlySalary.CreatedAt
            };
        }

        public async Task<List<EmployeeDto>> SearchAsync(EmployeeSearchCriteria criteria)
        {
            var query = _context.Employees
                .AsNoTracking()
                .Include(e => e.Grade)
                .Include(e => e.Position)
                .Where(e => criteria.IncludeDeleted || !e.IsDeleted);

            if (!string.IsNullOrWhiteSpace(criteria.FullName))
            {
                var term = criteria.FullName.Trim();
                query = query.Where(e => EF.Functions.ILike(e.FullName, $"%{term}%"));
            }

            if (criteria.GradeId.HasValue)
            {
                query = query.Where(e => e.GradeId == criteria.GradeId.Value);
            }

            if (criteria.PositionId.HasValue)
            {
                query = query.Where(e => e.PositionId == criteria.PositionId.Value);
            }

            if (criteria.HireDateFrom.HasValue)
            {
                query = query.Where(e => e.HireDate >= criteria.HireDateFrom.Value);
            }

            if (criteria.HireDateTo.HasValue)
            {
                query = query.Where(e => e.HireDate <= criteria.HireDateTo.Value);
            }

            query = criteria.SortBy switch
            {
                EmployeeSortBy.HireDate => criteria.SortDescending
                    ? query.OrderByDescending(e => e.HireDate)
                    : query.OrderBy(e => e.HireDate),
                EmployeeSortBy.FullName => criteria.SortDescending
                    ? query.OrderByDescending(e => e.FullName)
                    : query.OrderBy(e => e.FullName),
                _ => criteria.SortDescending
                    ? query.OrderByDescending(e => e.Id)
                    : query.OrderBy(e => e.Id)
            };

            // Сначала загружаем сущности, затем маппим в DTO
            var entities = await query.ToListAsync();
            var items = entities.Select(MapToEmployeeDto).ToList();

            return items;
        }

        private static EmployeeDto MapToEmployeeDto(Employee employee)
        {
            return new EmployeeDto
            {
                Id = employee.Id,
                FullName = employee.FullName,
                HireDate = employee.HireDate,
                GradeId = employee.GradeId,
                GradeName = employee.Grade?.GradeName,
                PositionId = employee.PositionId,
                PositionName = employee.Position?.PositionName,
                Notes = employee.Notes
            };
        }

        private static EmployeeDetailDto MapToEmployeeDetailDto(Employee employee)
        {
            return new EmployeeDetailDto
            {
                Id = employee.Id,
                FullName = employee.FullName,
                HireDate = employee.HireDate,
                GradeId = employee.GradeId,
                GradeName = employee.Grade?.GradeName,
                PositionId = employee.PositionId,
                PositionName = employee.Position?.PositionName,
                Notes = employee.Notes,
                MonthlySalaries = employee.MonthlySalaries
                    .OrderByDescending(ms => ms.SalaryDate)
                    .Select(MapToMonthlySalaryDto)
                    .ToList()
            };
        }

        private static MonthlySalaryDto MapToMonthlySalaryDto(MonthlySalary monthlySalary)
        {
            return new MonthlySalaryDto
            {
                Id = monthlySalary.Id,
                SalaryDate = monthlySalary.SalaryDate,
                Amount = monthlySalary.Amount,
                CreatedAt = monthlySalary.CreatedAt
            };
        }
    }
}
