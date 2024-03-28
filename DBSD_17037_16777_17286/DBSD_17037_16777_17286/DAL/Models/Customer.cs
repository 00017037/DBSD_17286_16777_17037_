using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DBSD_17037_16777_17286.DAL.Models
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        public int LoyaltyPoints { get; set; }
        public Person?  Person { get; set; }
        public int? PersonId { get; set; }

        public int TotalTransactionsAmount { get; set; }
    }
}
