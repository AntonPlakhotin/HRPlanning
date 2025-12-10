using System;
using System.ComponentModel.DataAnnotations;

namespace HRPlanning.Models
{
    public class Education
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;

        [Required]
        public string Type { get; set; } = null!;

        [Required]
        public string Name { get; set; } = null!;

        public string? IssuedBy { get; set; }

        public DateOnly? IssueDate { get; set; }

        public DateOnly? ExpiryDate { get; set; }
    }
}