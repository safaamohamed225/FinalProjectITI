using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Waseet.Models
{
    // Apartment entity
    public class Apartment
    {
        [Key]
        public int Code { get; set; }

        [Required]
        public string Description { get; set; }
        [Required]
        public string Location { get; set; }

        [Required, MaxLength(50)]
        public string Region { get; set; }

        [Required, Range(0, double.MaxValue)]
        public decimal ApartmentPrice { get; set; }

        [Required]
        public string Status { get; set; }
        [Required]
        public int Capacity { get; set; }

        [Required]
        public int NumofRoom { get; set; }

        public string GenderOfStudents { get; set; }
        [ForeignKey("Owner")]
        public int OwnerID { get; set; }


        // Navigation properties
        public ICollection<Rent> Rents { get; set; }
        public ICollection<Room> Rooms { get; set; }
        public Owner Owner { get; set; }
        public ICollection<ApprovalStatus> ApprovalStatuses { get; set; }
    }
}
