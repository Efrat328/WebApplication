using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string NameUser { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }

        // קשרים
        public ICollection<Project> Projects { get; set; }
        public ICollection<Task> Tasks { get; set; }
    }
}
