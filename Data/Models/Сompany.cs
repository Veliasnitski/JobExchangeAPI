using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    [Table("Companies")]
    public class Сompany
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
    }
}
