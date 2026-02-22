using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dto
{
    public class SubTaskDto
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int AssignedTo { get; set; }
        public string Status { get; set; }
        public DateTime Deadline { get; set; }
    }
}
