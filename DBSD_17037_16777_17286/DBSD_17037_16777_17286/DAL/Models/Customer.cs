using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DBSD_17037_16777_17286.DAL.Models
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }
        public int LoyaltyPoints { get; set; }
        public int PersonID { get; set; }

        [ForeignKey("PersonID")]
        public virtual Person ? Person { get; set; }
    }
}
