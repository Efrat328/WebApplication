using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dto
{
    public class ProjectDto
    {
        public string NameProject { get; set; }
        public string Description { get; set; }
        public ProjectStatus Status { get; set; }
        public DateTime Deadline { get; set; }
    }
}