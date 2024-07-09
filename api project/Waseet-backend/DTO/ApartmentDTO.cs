using System;

public class ApartmentDTO
{
    public string Description { get; set; }
    public string Region { get; set; }
    public int NumofRoom {  get; set; }
    public decimal ApartmentPrice { get; set; }
    public string GenderOfStudents { get; set; }
    public string Location { get; set; }
    public int Capacity { get; set; }
    public string OwnerID { get; set; }

    public IFormFileCollection images {  get; set; }
  


}
