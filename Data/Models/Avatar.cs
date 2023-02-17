using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    [Table("Avatars")]
    public class Avatar
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public byte[]? Image { get; set; }
    }
}
