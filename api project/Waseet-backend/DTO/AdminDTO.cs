using System.ComponentModel.DataAnnotations;

namespace Waseet.DTO
{
    public class AdminDTO
    {
        public int ID { get; set; }       
        public string SSN { get; set; }    
        public string F_name { get; set; }     
        public string L_name { get; set; }       
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
