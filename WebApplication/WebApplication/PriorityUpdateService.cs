using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Service.Interface;
using Service.Dto;
using TaskStatus = Repository.Entities.TaskStatus;

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
            // מחכה עד חצות
            var now = DateTime.Now;
            var nextRun = now.Date.AddDays(1); // חצות הלילה הבאה
            var delay = nextRun - now;
            await Task.Delay(delay, stoppingToken);

            // מריץ את עדכון העדיפויות
            await UpdatePriorities();
        }
    }

    private async Task UpdatePriorities()
    {
        using var scope = _serviceProvider.CreateScope();
        var taskService = scope.ServiceProvider.GetRequiredService<IService<TaskItemDto>>();

        var tasks = await taskService.GetAll();
        foreach (var task in tasks)
        {
            // רק משימות פתוחות או בביצוע
            if (task.Status == TaskStatus.Canceled || task.Status == TaskStatus.Completed)
                continue;

            var daysLeft = (task.Deadline - DateTime.Now).Days;

            if (daysLeft < task.Expected)
                task.Priority = TaskPriorityDto.High;
            else if (daysLeft < task.Expected * 2)
                task.Priority = TaskPriorityDto.Low;
            else
                task.Priority = TaskPriorityDto.Medium;

            await taskService.UpdateItem(task.Id, task);
        }
    }
}