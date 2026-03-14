using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Zא-ת ]+$",
        ErrorMessage = "The field can only contain Hebrew or English letters")]
        [MaxLength(20)]
        public string NameUser { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$",
        ErrorMessage = "Password must contain: uppercase letter, lowercase letter, number, special character, at least 8 characters")]
        public string Password { get; set; }

        [Required]
        public bool IsActive { get; set; }

        // connections
        public ICollection<Project> Projects { get; set; }
        public ICollection<TaskItem> Tasks { get; set; }
    }
}
