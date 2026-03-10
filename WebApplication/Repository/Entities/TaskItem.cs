using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public enum TaskStatus
    {
        Open,
        InProgress,
        Completed,
        Canceled
    }
    public enum TaskPriority
    {
        Low,
        Medium,
        High
    }
    public class TaskItem
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int AssignedTo { get; set; }
        public TaskPriority Priority { get; set; }
        public int Expected { get; set; }
        public TaskStatus Status { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime CompletedAt { get; set; }

        // connections
        public Project Project { get; set; }
        public User User { get; set; }
        public ICollection<SubTask> SubTasks { get; set; }
    }
}
