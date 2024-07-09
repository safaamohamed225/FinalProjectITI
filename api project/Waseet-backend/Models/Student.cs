using System.ComponentModel.DataAnnotations;

namespace Waseet.Models
{
    // Student entity
    public class Student
    {
        [Key]
        public int ID { get; set; }

        [Required, MaxLength(14)]
        public string SSN { get; set; }

        [Required, MaxLength(50)]
        public string F_name { get; set; }

        [Required, MaxLength(50)]
        public string L_name { get; set; }

        [Phone]
        public string Phone { get; set; }

        [Required, MaxLength(100)]
        public string Address { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public string ID_Identity { get; set; }

        // Navigation properties
        public ICollection<Rent> Rents { get; set; }
    }
}