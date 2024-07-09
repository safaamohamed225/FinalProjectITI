using System.ComponentModel.DataAnnotations;

namespace Waseet.Models
{
    // Room entity
    public class Room
    {
        [Key]
        public int ID { get; set; }

        [Required, MaxLength(50)]
        public string RoomNum { get; set; }

        [Required, Range(0, int.MaxValue)]
        public int NumOfBeds { get; set; }

        [Required, Range(0, double.MaxValue)]
        public decimal BedPrice { get; set; }

        // Foreign key
        public int ApartmentId { get; set; }

        // Navigation property
        public Apartment Apartment { get; set; }
    }
}
