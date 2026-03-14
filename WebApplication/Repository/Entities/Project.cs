using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required]
        [MaxLength(20)]
        public string NameProject { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }

        [Required]
        public ProjectStatus Status { get; set; }

        [Required]
        public DateTime Deadline { get; set; }

        // connections
        public ICollection<TaskItem> Tasks { get; set; }
    }
}
