using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRPlanning.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; } = null!;

        // »спользую DateOnly дл€ полей DATE (поддерживаетс€ EF Core + Npgsql в .NET 8)
        public DateOnly HireDate { get; set; }

        // Nullable FK Ч соответствует "ON DELETE SET NULL"
        public int? GradeId { get; set; }
        public Grade? Grade { get; set; }

        public int? PositionId { get; set; }
        public Position? Position { get; set; }

        public string? Notes { get; set; }

        public bool IsDeleted { get; set; } = false;

        public ICollection<MonthlySalary> MonthlySalaries { get; set; } = new List<MonthlySalary>();
        public ICollection<Education> Educations { get; set; } = new List<Education>();
        public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
    }
}