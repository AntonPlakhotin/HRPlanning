using System;
using System.ComponentModel.DataAnnotations;

namespace HRPlanning.Dto
{
    public class EmployeeCreateDto
    {
        [Required]
        public string FullName { get; set; } = null!;

        [Required]
        public DateOnly HireDate { get; set; }

        public int? GradeId { get; set; }
        public int? PositionId { get; set; }
        public string? Notes { get; set; }
    }
}

