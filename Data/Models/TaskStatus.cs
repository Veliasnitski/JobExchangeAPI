using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    [Table("TaskStatuses")]
    public class TaskStatus
    {
        public int Id { get; set; }
        public string? Description { get; set; }    
    }
}
