using System.ComponentModel.DataAnnotations;

namespace Waseet.Models
{
        // images multivalued attribute
    public class Apartment_Images
    {
            // Foreign keys
            public int ImageId { get; set; }
            public int ApartmentId { get; set; }
            public string sourse { get; set; }

        // Navigation properties
        public Apartment Apartment { get; set; }
    }
}
