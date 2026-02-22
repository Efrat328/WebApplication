using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string NameProject { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime Deadline { get; set; }

        // contextual properties
        public ICollection<TaskItem> Tasks { get; set; }
    }
}
