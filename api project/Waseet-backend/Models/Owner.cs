using System.ComponentModel.DataAnnotations;

namespace Waseet.Models
{
    // Owner entity
    public class Owner
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
        public string ID_Identity { get; set; }

        // Navigation properties
        public ICollection<Apartment> Apartments { get; set; }
    }
}