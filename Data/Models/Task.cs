using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    [Table("Tasks")]
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int? StatusId { get; set; }
        public int? CreaterId { get; set; }
        public int? AppointerId { get;}
        public int? ImplementerId { get; set;}
    }
}
