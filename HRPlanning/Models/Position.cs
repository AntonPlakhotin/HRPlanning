using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRPlanning.Models
{
    public class Position
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string PositionName { get; set; } = null!;

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}