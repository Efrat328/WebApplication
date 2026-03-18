using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

using Service.Services;
using TaskStatus = Repository.Entities.TaskStatus;
using Service.Dto;

namespace Service.Services
{
    public class PriorityUpdateService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public PriorityUpdateService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.Now;
                var nextRun = now.Date.AddDays(1);
                var delay = nextRun - now;
                await Task.Delay(delay, stoppingToken);

                await UpdatePriorities();
            }
        }

        private async Task UpdatePriorities()
        {
            using var scope = _serviceProvider.CreateScope();
            var taskService = scope.ServiceProvider.GetRequiredService<TaskService>();

            var tasks = await taskService.GetAll();
            foreach (var task in tasks)
            {
                if (task.Status == TaskStatus.Canceled || task.Status == TaskStatus.Completed)
                    continue;

                await taskService.UpdatePriority(task.Id, task);  // ✅ משתמשים בפונקציה הקיימת

                if (task.Priority == TaskPriorityDto.High)
                    await taskService.SplitIfHighPriority(task.Id);  // ✅ פיצול אוטומטי
            }
        }
    }
}