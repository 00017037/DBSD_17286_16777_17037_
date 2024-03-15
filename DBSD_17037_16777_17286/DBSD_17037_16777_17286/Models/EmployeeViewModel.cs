using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBSD_17037_16777_17286.Models
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Display(Name = "First Name")]
        public string ? FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string ? LastName { get; set; }

        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }

        [Display(Name = "Hourly Rate")]
        public decimal HourlyRate { get; set; }

        [Display(Name = "Married")]
        public bool IsMarried { get; set; }
        [NotMapped]
        [Display(Name = "Photo")]
        public IFormFile ? PhotoFile { get; set; }

        [Display(Name = "ManagerName")]
        public string? ManagerName { get; set; }

        [Display(Name = "ManagerSurname")]
        public string? ManagerSurname { get; set; }

        // Selectable Departments
        [Display(Name = "Department")]
        public string ? Depatment { get; set; }

        [Display(Name = "Department")]
        public int DepartmentId { get; set; }


    }
}
