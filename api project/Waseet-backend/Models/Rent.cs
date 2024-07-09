using System.Text.Json.Serialization;

namespace Waseet.Models
{
    // Rent join table entity
    public class Rent
    {
        // Foreign keys
        public int StudentId { get; set; }
        public int ApartmentId { get; set; }

        // Navigation properties

        [JsonIgnore]
        public Student Student { get; set; }
        [JsonIgnore]
        public Apartment Apartment { get; set; }
        public DateTime Date { get; set; }
    }
}
