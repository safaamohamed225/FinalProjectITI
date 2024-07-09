using System.ComponentModel.DataAnnotations;

namespace Waseet.DTO
{
    public class RequestDTO
    {
        public int ID { get; set; }
        public string StudentId { get; set; }
        public int ApartmentId { get; set; }
        public string Status { get; set; }
    }
}
