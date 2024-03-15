using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBSD_17037_16777_17286.DAL.Models
{
    public class Person
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        public string ContactDetails { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
