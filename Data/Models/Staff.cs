using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    [Table("Staff")]
    public class Staff
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set;}
        public DateTime DataOfBirth { get; set;}
        public string? Phone { get; set;}
        public string? Email { get; set; }
        public int? DepartmentId { get; set; }
        public int? SpecId { get; set; }
        public int? RoleId { get; set; }
        public int? AvatarId { get; set; }
    }
}
