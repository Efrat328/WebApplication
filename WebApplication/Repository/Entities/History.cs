using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace Repository.Entities
{
    public enum HistoryStatus
    {
        Open,
        InProgress,
        Completed,
        Canceled
    }
    public class History
    {
        public int Id { get; set; }
        [Required]
        public int SubTaskId { get; set; }
        [Required]
        public HistoryStatus OldStatus { get; set; }
        [Required]
        public HistoryStatus NewStatus { get; set; }
        [Required] 
        public DateTime ChangedAt { get; set; }
        public bool IsActive { get; set; }= true;

        // connections
        public SubTask SubTask { get; set; }
    }
}
