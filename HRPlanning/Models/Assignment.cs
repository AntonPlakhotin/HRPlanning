using System;
using System.ComponentModel.DataAnnotations;

namespace HRPlanning.Models
{
    public class Assignment
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;

        public int TeamId { get; set; }
        public Team Team { get; set; } = null!;

        [Required]
        public string RoleInTeam { get; set; } = null!;

        public DateOnly AssignmentDate { get; set; }
    }
}