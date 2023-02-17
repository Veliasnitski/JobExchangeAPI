using Data.Models;
using Microsoft.EntityFrameworkCore;
using Task = Data.Models.Task;
using TaskStatus = Data.Models.TaskStatus;

namespace Data
{
    public class JobExchangeDBContext : DbContext
    {
        public JobExchangeDBContext(DbContextOptions<JobExchangeDBContext> options) : base(options) => Database.EnsureCreated();

        public DbSet<Сompany> Companies { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Avatar> Avatars { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<TaskStatus> TaskStatuses { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
