using System;

namespace HRPlanning.Dto
{
    public class MonthlySalaryDto
    {
        public int Id { get; set; }
        public DateOnly SalaryDate { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

