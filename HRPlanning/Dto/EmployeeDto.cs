using System;

namespace HRPlanning.Dto
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public DateOnly HireDate { get; set; }
        public int? GradeId { get; set; }
        public string? GradeName { get; set; }
        public int? PositionId { get; set; }
        public string? PositionName { get; set; }
        public string? Notes { get; set; }
    }
}

