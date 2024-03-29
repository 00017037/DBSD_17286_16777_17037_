namespace DBSD_17037_16777_17286.Models
{
    public class EmployeeFilterModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DepartmentName { get; set; }
        public DateTime? HireDate { get; set; }
        public string SortField { get; set; }
        public bool SortDesc { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

        public int TotalPages { get; set; }

        public IEnumerable<EmployeeViewModel> Employees { get; set; }
    }
}
