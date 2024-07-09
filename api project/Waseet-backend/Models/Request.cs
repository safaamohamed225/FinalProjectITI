using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Waseet.Models
{
    // Request join table entity
    public class Request
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        // Foreign keys
        public int StudentId { get; set; }
        public int ApartmentId { get; set; }

        [Required]
        public string Status { get; set; }

        // Navigation properties
        public Student Student { get; set; }
        public Apartment Apartment { get; set; }
    }
}
