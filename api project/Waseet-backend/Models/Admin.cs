using System.ComponentModel.DataAnnotations;

namespace Waseet.Models
{
    // Admin entity
    public class Admin
    {
        [Key]
        public int ID { get; set; }

        [Required, MaxLength(14)]
        public string SSN { get; set; }

        [Required, MaxLength(50)]
        public string F_name { get; set; }

        [Required, MaxLength(50)]
        public string L_name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Phone { get; set; }

        // Navigation properties
        public ICollection<ApprovalStatus> ApprovalStatuses { get; set; }
    }
}