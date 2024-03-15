using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore;

namespace DBSD_17037_16777_17286.DAL.Models
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }
        
        public DateTime Date { get; set; }
        
        public decimal Total { get; set; }

      
        public virtual Customer Customer { get; set; }
        public int CustomerId { get; set; }
        public virtual Employee  Employee { get; set; }
        public int EmployeeId { get; set; }


    }


}
