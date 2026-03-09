using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskStatus = Repository.Entities.TaskStatus;


namespace Service.Dto
{
    public class TaskItemDto
    {
        public string ProjectName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int AssignedTo { get; set; }
        //public int AssignedToId { get; set; }   // for save (FK)

        public string Priority { get; set; }
        public TaskStatus Status { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime Deadline { get; set; }
    }
}
