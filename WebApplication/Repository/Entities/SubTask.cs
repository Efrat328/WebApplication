using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public enum SubTaskStatus
    {
        Open,
        InProgress,
        Completed,
        Canceled
    }
    public class SubTask
    {
        public int Id { get; set; }
        public int TaskId { get; set; }

        [Required]
        [MaxLength(20)]
        public string Title { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }

        [Required]
        public int AssignedTo { get; set; }

        [Required]
        public SubTaskStatus Status { get; set; }

        //[Required]
        //public DateTime Deadline { get; set; }

        [Required]
        public DateTime CompletedAt { get; set; }

        // connections
        public TaskItem Tasks { get; set; }
        public User User { get; set; }
        public ICollection<History> Histories { get; set; }
    }
}
