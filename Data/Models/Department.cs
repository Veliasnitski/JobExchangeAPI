using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    [Table("Departments")]
    public class Department
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int CompanyId { get; set; }
    }
}
