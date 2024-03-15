using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DBSD_17037_16777_17286.DAL.Models
{
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual Employee ? Manager { get; set; }
        public int? ManagerId { get; set; }

    }
}
