using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    [Table("Specialties")]
    public class Specialty
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Responsibilities { get; set; } 
    }
}
