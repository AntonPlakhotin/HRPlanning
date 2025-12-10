using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRPlanning.Models
{
    public class Team
    {
        [Key]
        public int Id { get; set; }

        public int? StructureTemplateId { get; set; }
        public StructureTemplate? StructureTemplate { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
    }
}