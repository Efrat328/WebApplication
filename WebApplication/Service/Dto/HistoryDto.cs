using System;
using System.Collections.Generic;
using System.Linq;
namespace Service.Dto
{
    public class HistoryDto
    {
        public int Id { get; set; }
        public int SubTaskId { get; set; }
        public string OldStatus { get; set; }
        public string NewStatus { get; set; }
        public DateTime ChangedAt { get; set; }
    }
}