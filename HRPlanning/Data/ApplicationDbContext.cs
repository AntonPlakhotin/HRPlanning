using Microsoft.EntityFrameworkCore;
using HRPlanning.Models;
using System;

namespace HRPlanning.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Grade> Grades { get; set; } = null!;
        public DbSet<Position> Positions { get; set; } = null!;
        public DbSet<MonthlySalary> MonthlySalaries { get; set; } = null!;
        public DbSet<Education> Educations { get; set; } = null!;
        public DbSet<StructureTemplate> StructureTemplates { get; set; } = null!;
        public DbSet<Team> Teams { get; set; } = null!;
        public DbSet<Assignment> Assignments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Employees: GradeId и PositionId nullable, ON DELETE SET NULL
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Grade)
                .WithMany(g => g.Employees)
                .HasForeignKey(e => e.GradeId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Position)
                .WithMany(p => p.Employees)
                .HasForeignKey(e => e.PositionId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Employee>()
                .Property(e => e.IsDeleted)
                .HasDefaultValue(false);

            // MonthlySalaries: уникальный индекс (EmployeeId, SalaryDate), CreatedAt default NOW()
            modelBuilder.Entity<MonthlySalary>()
                .HasIndex(ms => new { ms.EmployeeId, ms.SalaryDate })
                .IsUnique();

            modelBuilder.Entity<MonthlySalary>()
                .HasOne(ms => ms.Employee)
                .WithMany(e => e.MonthlySalaries)
                .HasForeignKey(ms => ms.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MonthlySalary>()
                .Property(ms => ms.CreatedAt)
                .HasDefaultValueSql("NOW()");

            // Education
            modelBuilder.Entity<Education>()
                .HasOne(ed => ed.Employee)
                .WithMany(e => e.Educations)
                .HasForeignKey(ed => ed.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // StructureTemplates.CreatedAt default CURRENT_DATE (если хотите)
            modelBuilder.Entity<StructureTemplate>()
                .Property(st => st.IsDeleted)
                .HasDefaultValue(false);

            modelBuilder.Entity<StructureTemplate>()
                .Property(st => st.CreatedAt)
                .HasDefaultValueSql("CURRENT_DATE");

            // Teams -> StructureTemplate (nullable, ON DELETE SET NULL)
            modelBuilder.Entity<Team>()
                .HasOne(t => t.StructureTemplate)
                .WithMany(st => st.Teams)
                .HasForeignKey(t => t.StructureTemplateId)
                .OnDelete(DeleteBehavior.SetNull);

            // Assignments: уникальный индекс (EmployeeId, TeamId), каскад при удалении Employee/Team
            modelBuilder.Entity<Assignment>()
                .HasIndex(a => new { a.EmployeeId, a.TeamId })
                .IsUnique();

            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.Employee)
                .WithMany(e => e.Assignments)
                .HasForeignKey(a => a.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.Team)
                .WithMany(t => t.Assignments)
                .HasForeignKey(a => a.TeamId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}