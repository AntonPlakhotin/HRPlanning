using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRPlanning.Models
{
    public class StructureTemplate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public bool IsDeleted { get; set; } = false;

        // DATE -> DateOnly
        public DateOnly CreatedAt { get; set; }

        public ICollection<Team> Teams { get; set; } = new List<Team>();
    }
}