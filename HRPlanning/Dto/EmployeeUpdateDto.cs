using System;

namespace HRPlanning.Dto
{
    public class EmployeeUpdateDto
    {
        public string? FullName { get; set; }
        public DateOnly? HireDate { get; set; }
        public int? GradeId { get; set; }
        public int? PositionId { get; set; }
        public string? Notes { get; set; }
    }
}

