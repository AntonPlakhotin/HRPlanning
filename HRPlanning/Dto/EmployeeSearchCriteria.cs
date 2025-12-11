using System;

namespace HRPlanning.Dto
{
    public class EmployeeSearchCriteria
    {
        public string? FullName { get; set; }
        public int? GradeId { get; set; }
        public int? PositionId { get; set; }
        public DateOnly? HireDateFrom { get; set; }
        public DateOnly? HireDateTo { get; set; }
        public bool IncludeDeleted { get; set; } = false;
        public EmployeeSortBy SortBy { get; set; } = EmployeeSortBy.Id;
        public bool SortDescending { get; set; } = false;
    }

    public enum EmployeeSortBy
    {
        Id = 0,
        FullName = 1,
        HireDate = 2
    }
}
