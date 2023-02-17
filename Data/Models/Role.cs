using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    [Table("Roles")]
    public class Role
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Responsibilities { get; set; }
    }
}
