using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Service.Dto
{
    public class HistoryDto
    {
        public int Id { get; set; }
        public int SubTaskId { get; set; }
        public HistoryStatus OldStatus { get; set; }
        public HistoryStatus NewStatus { get; set; }
        public DateTime ChangedAt { get; set; }
    }
}