using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBSD_17037_16777_17286.Models
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }

        [Display(Name = "Hourly Rate")]
        public decimal HourlyRate { get; set; }

        [Display(Name = "Married")]
        public bool IsMarried { get; set; }
        [NotMapped]
        [Display(Name = "Photo")]
        public IFormFile PhotoFile { get; set; }

        [Display(Name = "Manager")]
        public int? ManagerId { get; set; }

        // Selectable Departments
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
    }
}
