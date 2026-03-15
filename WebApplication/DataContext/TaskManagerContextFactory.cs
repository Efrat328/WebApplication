using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataContext
{
    public class TaskManagerContextFactory : IDesignTimeDbContextFactory<TaskManagerContext>
    {
        public TaskManagerContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TaskManagerContext>();
            optionsBuilder.UseSqlServer("Server=DESKTOP-1VUANBN;Database=TaskManagerDB;Trusted_Connection=True;TrustServerCertificate=True");
            return new TaskManagerContext(optionsBuilder.Options);
        }
    }
}