using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataContext
{
    public class TaskManagerContextFactory : IDesignTimeDbContextFactory<TaskManagerContext>
    {
        public TaskManagerContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TaskManagerContext>();
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=TaskManager;User Id=sa;Password=YourPassword123!;TrustServerCertificate=True");

            return new TaskManagerContext(optionsBuilder.Options);
        }
    }
}