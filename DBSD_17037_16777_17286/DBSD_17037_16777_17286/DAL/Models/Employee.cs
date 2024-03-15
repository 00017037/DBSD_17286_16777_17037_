using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DBSD_17037_16777_17286.DAL.Models
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id{ get; set; }
        public DateTime HireDate { get; set; }
        public decimal HourlyRate { get; set; }

        public bool isMaried { get; set; }

         public byte[]? Photo { get; set; }


        public virtual Department Department { get; set; }

        public int DepartmentId { get; set; }

        public virtual  Employee Manager { get; set; } // Self-referencing relationship
        public int? ManagerId { get; set; } // Nullable for employees without managers

        public virtual Person Person { get; set; }

        public int PersonId { get; set; }

    }
}
