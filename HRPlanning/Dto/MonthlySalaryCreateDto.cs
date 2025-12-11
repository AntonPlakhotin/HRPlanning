using System;
using System.ComponentModel.DataAnnotations;

namespace HRPlanning.Dto
{
    public class MonthlySalaryCreateDto
    {
        [Required]
        public DateOnly SalaryDate { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }
    }
}
