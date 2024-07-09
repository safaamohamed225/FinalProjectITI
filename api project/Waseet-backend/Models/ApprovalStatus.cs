namespace Waseet.Models
{
    // ApprovalStatus join table entity
    public class ApprovalStatus
    {
        // Foreign keys
        public int ApartmentId { get; set; }
        public int AdminId { get; set; }

        // Navigation properties
        public Apartment Apartment { get; set; }
        public Admin Admin { get; set; }
    }
}
