using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Org.BouncyCastle.Asn1.Ocsp;
using Waseet.DTO;
using Waseet.Helper;
using Waseet.Models;
using Waseet.Repsitory.RequestRepo;
using Waseet.Service;
using Request = Waseet.Models.Request;

namespace Waseet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequestRepo ds;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmailService _emailService;
        private readonly RentalContext context;

        public RequestController(IRequestRepo ds, UserManager<ApplicationUser> _userManager , IEmailService emailService, RentalContext context)
        {
            this.ds = ds;
            this.userManager = _userManager;
            _emailService = emailService;
            this.context = context;
        }

        //API To Make Request
        [HttpPost("{studentID}/{apartmentID:int}")] //student ID from identity
        //public IActionResult MakeRequest(int studentID, int apartmentID)
        public async Task<IActionResult> MakeRequest(string studentID, int apartmentID)
        {
            int st_ID = await ds.GetStudentID(studentID);

            if (ds.ISvalidAparmentID(apartmentID))
            {
                if (ds.ISvalidStudentID(st_ID))
                {
                    if (ds.IsRepeatedRequest(st_ID, apartmentID))
                    {
                        return BadRequest("Repeated Rent");
                    }
                    else
                    {
                        ds.makeRequest(st_ID, apartmentID);                        
                        return Ok();                                              
                    }

                }
                return BadRequest();
            }
            return BadRequest();




            //if (!ds.IsEmptyTable())
            //{
            //    if (ds.ISvalidAparmentID(apartmentID))
            //    {
            //        if (ds.ISvalidStudentID(studentID))
            //        {
            //            if (ds.IsExistedAparmentID(apartmentID))
            //            {
            //                if (ds.IExistedStudentID(studentID))
            //                {
            //                    ds.makeRequest(studentID, apartmentID);
            //                    return Ok($"Request Recorded Successfully for Student : {studentID} to Apartment : {apartmentID}");
            //                }
            //                return BadRequest("Invalid Student ID");
            //            }
            //            return BadRequest("Invalid Apartment ID");

            //        }
            //        return BadRequest("Invalid Student ID");
            //    }
            //    return BadRequest("Invalid Apartment ID");
            //}
            //else
            //{
            //    if (ds.ISvalidAparmentID(apartmentID))
            //    {
            //        if (ds.ISvalidStudentID(studentID))
            //        {
            //            ds.makeRequest(studentID, apartmentID);
            //            return Ok($"Request Recorded Successfully for Student : {studentID} to Apartment : {apartmentID}");
            //        }
            //        return BadRequest("Invalid Student ID");
            //    }
            //    return BadRequest("Invalid Apartment ID");
            //}

        }



        //API To Approve Request
        [HttpPost("apprvalfrombody")]
        public async Task<IActionResult> ApproveRequest([FromBody] RequestDTO requestDTO)
        {
            int st_ID = await ds.GetStudentID(requestDTO.StudentId);
            if (ModelState.IsValid)
            {
                Models.Request request = new Models.Request();
                request.ID = requestDTO.ID;
                request.StudentId = st_ID;
                request.ApartmentId = requestDTO.ApartmentId;
                request.Status = requestDTO.Status;
                if (ds.IExistedRequesttID(request.ID))
                {
                    if (ds.IsRentedApartment(request.ApartmentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        if (ds.IsApprovedRequest(request.ID))
                            return BadRequest();
                        else
                        {

                            return Ok(ds.approveRequestById(request.ID));
                        }

                    }

                }
                return NotFound("Request Not Found");
            }
            else
            {
                return BadRequest(ModelState);
            }

        }



        //API To Approve RequestById
        [HttpPost("approvalfromurl/{RequestID:int}/{ApartmentID:int}")]
        //public IActionResult ApproveRequest(int RequestID, int ApartmentID)
        public IActionResult ApproveRequest(int RequestID, int ApartmentID)
        {
            if (ds.IExistedRequesttID(RequestID))
            {
                if (ds.IsRentedApartment(ApartmentID))
                {
                    return NotFound("Request Contain Already Rented Apartment");
                }
                else
                {
                    if (ds.IsApprovedRequest(RequestID))
                    {                       
                        return BadRequest("Already Approved Request");
                    }
                        
                    else
                    {

                        return Ok(ds.approveRequestById(RequestID));


                    }

                }

            }
            return NotFound("Request Not Found");
        }


        //string email = ds.GetUserEmail(RequestID);
        //
        //Mailrequest mailrequest = new Mailrequest();
        //mailrequest.ToEmail = email;
        //mailrequest.Subject = "Aprrove Request";
        //mailrequest.Body = "Your Request Approved Sucessfully!";
        //_emailService.SendEmailAsync(mailrequest);/////
        //
        //return Ok(ds.approveRequestById(RequestID));







        //API To Get Requests per Student
        [HttpGet("GetRequestsPerStudent/{studentID}")]
        public async Task<IActionResult> GetRequestsperStudent(string studentID)
        {
            int st_ID = await ds.GetStudentID(studentID);
            if (ds.ISvalidStudentID(st_ID))
            {
                if (ds.ISvalidStudentID(st_ID))
                {
                    List<Models.Request> requestsPerStudent = await ds.GetRequestsByPerStudent(st_ID);
                    return Ok(requestsPerStudent);
                }
                return NotFound("Invalid Student Selection!");
            }
            return NotFound("Invalid Student Selection!");
        }



        //API To Get Requests per Apartment
        [HttpGet("GetRequestsPerApartment/{ApartmentID:int}")]
        public async Task<IActionResult> GetRequestsperApartment(int ApartmentID)
        {
            if (ds.ISvalidAparmentID(ApartmentID))
            {
                if (ds.IsExistedAparmentID(ApartmentID))
                {
                    List<Models.Request> requestsPerApartment = await ds.GetRequestsByPerApartment(ApartmentID);
                    return Ok(requestsPerApartment);
                }
                return NotFound("Invalid Apartment Selection!");
            }
            return NotFound("Invalid Apartment Selection!");
        }


        //API to Get All Requests
        [HttpGet("GetAllRequests")]
        public IActionResult GetAllRequests()
        {
            return Ok(ds.GetAllRequests());
        }


        //API to Get All Requests Related To User(Owner) Apartments
        [HttpGet("GetAllRequestsRelatedToSpecificOwner/{UserID}")]
        public IActionResult GetAllRequestsRelatedToUserApartments(string UserID)
         {
            if (string.IsNullOrEmpty(UserID))
            {
                return BadRequest("Invalid Owner Selection! Please Re Login");
            }
            else
            {
                List<Request> RequestsRelated = ds.GetAllRequestsRelatedToUserApartments(UserID);
                List<RequestApartmentDTO> resultRequests = new List<RequestApartmentDTO>();
                foreach (Request request in RequestsRelated)
                {
                    Apartment apartment = context.Apartments.SingleOrDefault(a => a.Code == request.ApartmentId);
                    if (apartment != null)
                    {
                        RequestApartmentDTO dto = new RequestApartmentDTO();
                        dto.ID = request.ID;
                        dto.StudentId = request.StudentId;
                        dto.ApartmentId= request.ApartmentId;
                        dto.Status = request.Status;
                        dto.Region = apartment.Region;
                        dto.Price = apartment.ApartmentPrice;
                        dto.Gendre = apartment.GenderOfStudents;
                        dto.NumOfRooms = apartment.NumofRoom;
                        resultRequests.Add(dto);
                    }
                }
                return Ok(resultRequests);
                //return Ok(ds.GetAllRequestsRelatedToUserApartments(UserID));
            }
        }


        //API to Get All Approved Requests
        [HttpGet("GetAllApprovedRequests")]
        public IActionResult GetAllApprovedRequests()
        {
            return Ok(ds.GetAllApprovedRequests());
        }



        //API to Get All Pending Requests
        [HttpGet("GetAllPendingRequests")]
        public IActionResult GetAllPendingRequests()
        {
            return Ok(ds.GetAllPendingRequests());
        }


        //API to Get All Approved Requests Per Student
        [HttpGet("GetAllApprovedRequestsPerStudent/{StudentID}")]
        public async Task<IActionResult> GetAllApprovedRequestsPerStudent(string StudentID)
        {
            var Valid_Student = await userManager.FindByIdAsync(StudentID);
            if (Valid_Student != null)
            {
                List<Req_RoomDTO> result = new List<Req_RoomDTO>();
                if (string.IsNullOrEmpty(StudentID))
                {
                    return NotFound("Invalid Student Selection");
                }
                else
                {
                    var requests = await ds.GetAllApprovedRequestsPerStudent(StudentID);
                    foreach (var req in requests)
                    {
                        Req_RoomDTO req_roomDto = new Req_RoomDTO();
                        req_roomDto.RequestId = req.ID;
                        req_roomDto.Region = req.Apartment.Region;
                        req_roomDto.Price = req.Apartment.ApartmentPrice;
                        req_roomDto.Capacity = req.Apartment.Capacity;
                        result.Add(req_roomDto);
                    }
                    return Ok(result);
                }
            }
            else
            {
                return NotFound("Invalid Student Selection");
            }
        }


        //API to Get All Pending Requests Per Student
        [HttpGet("GetAllPendingRequestsPerStudent/{StudentID}")]
        public async Task<IActionResult> GetAllPendingRequestsPerStudent(string StudentID)
        {
            var Valid_Student = await userManager.FindByIdAsync(StudentID);
            if (Valid_Student != null)
            {
                List<Req_RoomDTO> result = new List<Req_RoomDTO>();
                if (string.IsNullOrEmpty(StudentID))
                {
                    return NotFound("Invalid Student Selection");
                }
                else
                {
                    var requests = await ds.GetAllPendingRequestsPerStudent(StudentID);
                    foreach (var req in requests)
                    {
                        Req_RoomDTO req_roomDto = new Req_RoomDTO();
                        req_roomDto.RequestId = req.ID;
                        req_roomDto.Region = req.Apartment.Region;
                        req_roomDto.Price = req.Apartment.ApartmentPrice;
                        req_roomDto.Capacity = req.Apartment.Capacity;
                        result.Add(req_roomDto);
                    }
                    return Ok(result);
                }
            }
            else
            {
                return NotFound("Invalid Student Selection");
            }
        }




        [HttpDelete("{RquestId}")]
        public IActionResult DeleteRequest(int RquestId)
        {
            if (ds.IExistedRequesttID(RquestId))
            {
                return Ok(ds.deleteRequest(RquestId));
            }
            return NotFound("Invalid Request Selection!");
        }        
    }
}
