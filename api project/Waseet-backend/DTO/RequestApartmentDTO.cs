namespace Waseet.DTO
{
    public class RequestApartmentDTO
    {
        public int ID { get; set; }      
        public int StudentId { get; set; }
        public int ApartmentId { get; set; }
        public string Status { get; set; }
        public string Region { get; set; }
        public decimal Price { get; set; }
        public string Gendre { get; set; }
        public int NumOfRooms { get; set; }
    }
}
