using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string NameUser { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }
}
