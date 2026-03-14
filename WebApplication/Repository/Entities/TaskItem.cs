using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


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
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        [Required]
        public int AssignedTo { get; set; }
        public TaskPriority Priority { get; set; }
        [Range(1, 100)]
        public int Expected { get; set; }//expected time to complete in days
        public TaskStatus Status { get; set; }
        [Required]
        public DateTime Deadline { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime CompletedAt { get; set; }

        // connections
        public Project Project { get; set; }
        public User User { get; set; }
        public ICollection<SubTask> SubTasks { get; set; }
    }
}
