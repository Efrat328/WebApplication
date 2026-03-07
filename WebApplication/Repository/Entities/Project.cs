using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public enum ProjectStatus 
    {
        Open,
        InProgress,
        Completed,
        Canceled
    }
    public class Project
    {
        public int Id { get; set; }
        public string NameProject { get; set; }
        public string Description { get; set; }
        public ProjectStatus Status { get; set; }
        public DateTime Deadline { get; set; }

        // connections
        public ICollection<TaskItem> Tasks { get; set; }
    }
}
