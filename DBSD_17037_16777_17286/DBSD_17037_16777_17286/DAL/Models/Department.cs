using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DBSD_17037_16777_17286.DAL.Models
{
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }
        public int? ManagerID { get; set; }
        public string Name { get; set; }

        [ForeignKey("ManagerID")]
        public virtual Employee ? Manager { get; set; } 
    }
}
