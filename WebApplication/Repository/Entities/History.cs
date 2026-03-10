using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public int SubTaskId { get; set; }
        public HistoryStatus OldStatus { get; set; }
        public HistoryStatus NewStatus { get; set; } 
        public DateTime ChangedAt { get; set; }
        public bool IsActive { get; set; }

        // connections
        public SubTask SubTask { get; set; }
    }
}
