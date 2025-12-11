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

            // “аблицы в lower_case
            modelBuilder.Entity<Grade>().ToTable("grade");
            modelBuilder.Entity<Position>().ToTable("position");
            modelBuilder.Entity<Employee>().ToTable("employees");
            modelBuilder.Entity<MonthlySalary>().ToTable("monthlysalaries");
            modelBuilder.Entity<Education>().ToTable("education");
            modelBuilder.Entity<StructureTemplate>().ToTable("structuretemplates");
            modelBuilder.Entity<Team>().ToTable("teams");
            modelBuilder.Entity<Assignment>().ToTable("assignments");

            // явное маппирование колонок в lower_case Ч соответствует некавычекным идентификаторам в Postgres
            modelBuilder.Entity<Grade>(b =>
            {
                b.Property(p => p.Id).HasColumnName("id");
                b.Property(p => p.GradeName).HasColumnName("grade");
            });

            modelBuilder.Entity<Position>(b =>
            {
                b.Property(p => p.Id).HasColumnName("id");
                b.Property(p => p.PositionName).HasColumnName("position");
            });

            modelBuilder.Entity<Employee>(b =>
            {
                b.Property(e => e.Id).HasColumnName("id");
                b.Property(e => e.FullName).HasColumnName("fullname");
                b.Property(e => e.HireDate).HasColumnName("hiredate");
                b.Property(e => e.GradeId).HasColumnName("gradeid");
                b.Property(e => e.PositionId).HasColumnName("positionid");
                b.Property(e => e.Notes).HasColumnName("notes");
                b.Property(e => e.IsDeleted).HasColumnName("isdeleted").HasDefaultValue(false);
            });

            modelBuilder.Entity<MonthlySalary>(b =>
            {
                b.Property(ms => ms.Id).HasColumnName("id");
                b.Property(ms => ms.EmployeeId).HasColumnName("employeeid");
                b.Property(ms => ms.SalaryDate).HasColumnName("salarydate");
                b.Property(ms => ms.Amount).HasColumnName("amount").HasColumnType("numeric(12,2)");
                b.Property(ms => ms.CreatedAt).HasColumnName("createdat").HasDefaultValueSql("NOW()");
                b.HasIndex(ms => new { ms.EmployeeId, ms.SalaryDate }).IsUnique();
            });

            modelBuilder.Entity<Education>(b =>
            {
                b.Property(ed => ed.Id).HasColumnName("id");
                b.Property(ed => ed.EmployeeId).HasColumnName("employeeid");
                b.Property(ed => ed.Type).HasColumnName("type");
                b.Property(ed => ed.Name).HasColumnName("name");
                b.Property(ed => ed.IssuedBy).HasColumnName("issuedby");
                b.Property(ed => ed.IssueDate).HasColumnName("issuedate");
                b.Property(ed => ed.ExpiryDate).HasColumnName("expirydate");
            });

            modelBuilder.Entity<StructureTemplate>(b =>
            {
                b.Property(st => st.Id).HasColumnName("id");
                b.Property(st => st.Name).HasColumnName("name");
                b.Property(st => st.Description).HasColumnName("description");
                b.Property(st => st.IsDeleted).HasColumnName("isdeleted").HasDefaultValue(false);
                b.Property(st => st.CreatedAt).HasColumnName("createdat").HasDefaultValueSql("CURRENT_DATE");
            });

            modelBuilder.Entity<Team>(b =>
            {
                b.Property(t => t.Id).HasColumnName("id");
                // в вашей DDL им€ колонки Ч StructureTemplatesId -> lower_case: structuretemplatesid
                b.Property(t => t.StructureTemplateId).HasColumnName("structuretemplatesid");
                b.Property(t => t.Name).HasColumnName("name");
                b.Property(t => t.Description).HasColumnName("description");
            });

            modelBuilder.Entity<Assignment>(b =>
            {
                b.Property(a => a.Id).HasColumnName("id");
                b.Property(a => a.EmployeeId).HasColumnName("employeeid");
                b.Property(a => a.TeamId).HasColumnName("teamid");
                b.Property(a => a.RoleInTeam).HasColumnName("roleinteam");
                b.Property(a => a.AssignmentDate).HasColumnName("assignmentdate");
                b.HasIndex(a => new { a.EmployeeId, a.TeamId }).IsUnique();
            });

            // —в€зи (по модели)
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

            modelBuilder.Entity<MonthlySalary>()
                .HasOne(ms => ms.Employee)
                .WithMany(e => e.MonthlySalaries)
                .HasForeignKey(ms => ms.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Education>()
                .HasOne(ed => ed.Employee)
                .WithMany(e => e.Educations)
                .HasForeignKey(ed => ed.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Team>()
                .HasOne(t => t.StructureTemplate)
                .WithMany(st => st.Teams)
                .HasForeignKey(t => t.StructureTemplateId)
                .OnDelete(DeleteBehavior.SetNull);

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