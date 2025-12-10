using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRPlanning.Models
{
    public class MonthlySalary
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;

        // SalaryDate — DATE
        public DateOnly SalaryDate { get; set; }

        [Column(TypeName = "numeric(12,2)")]
        public decimal Amount { get; set; }

        // TIMESTAMP -> DateTime
        public DateTime CreatedAt { get; set; }
    }
}