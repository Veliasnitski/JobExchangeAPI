using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Data.Models
{
    [Table("Users")]
    public class User
    {
        [JsonIgnore]
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }   
    }
}
