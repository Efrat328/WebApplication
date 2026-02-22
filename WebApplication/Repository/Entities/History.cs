using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public class History
    {
        public int Id { get; set; }
        public int SubTaskId { get; set; }
        public string OldStatus { get; set; }
        public string NewStatus { get; set; }
        public DateTime ChangedAt { get; set; }

        // context
        public SubTask SubTask { get; set; }
    }
}
