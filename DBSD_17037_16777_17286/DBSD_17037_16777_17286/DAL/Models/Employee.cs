using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DBSD_17037_16777_17286.DAL.Models
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID{ get; set; }
        public int DepartmentID { get; set; }
        public DateTime HireDate { get; set; }
        public decimal HourlyRate { get; set; }
        public int? ManagerID { get; set; } // Nullable for employees without managers
        public int PersonID { get; set; }

        public bool isMaried { get; set; }

         public byte[] Photo { get; set; }


        [ForeignKey("DepartmentID")]
        public virtual Department Department { get; set; }

        [ForeignKey("ManagerID")]
        public virtual Employee Manager { get; set; } // Self-referencing relationship

        [ForeignKey("PersonID")]
        public virtual Person Person { get; set; }


     

    }
}
