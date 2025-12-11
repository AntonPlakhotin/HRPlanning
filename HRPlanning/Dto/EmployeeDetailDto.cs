using System;
using System.Collections.Generic;

namespace HRPlanning.Dto
{
    public class EmployeeDetailDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public DateOnly HireDate { get; set; }
        public int? GradeId { get; set; }
        public string? GradeName { get; set; }
        public int? PositionId { get; set; }
        public string? PositionName { get; set; }
        public string? Notes { get; set; }
        public IReadOnlyList<MonthlySalaryDto> MonthlySalaries { get; set; } = Array.Empty<MonthlySalaryDto>();
    }
}

