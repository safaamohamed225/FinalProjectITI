using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Org.BouncyCastle.Asn1.X509;
using Waseet.DTO;
using Waseet.Helper;
using Waseet.Models;
using Waseet.Service;

namespace Waseet.Repsitory.RequestRepo
{
    public class RequestRepo : IRequestRepo
    {
        private readonly RentalContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmailService _emailService;


        public RequestRepo(RentalContext context, UserManager<ApplicationUser> _userManager, IEmailService emailService)
        {
            this.context = context;
            userManager = _userManager;
            _emailService = emailService;
        }


        public void makeRequest(int StudentID, int ApartmentID)
        {
            Request request = new Request();
            request.ApartmentId = ApartmentID;
            request.StudentId = StudentID;
            request.Status = "pending";
            try
            {
                context.Requests.Add(request);
                context.SaveChanges();
            }
            catch
            {

            }
            

            try
            {
                string email = GetUserEmailByUserID(StudentID);
                Mailrequest mailrequest = new Mailrequest();
                mailrequest.ToEmail = email;
                mailrequest.Subject = "make Request ";
                mailrequest.Body = "Your Request send Sucessfully!";
                
                _emailService.SendEmailAsync(mailrequest);
            }
            catch (Exception ex)
            {


            }

            try
            {
                Apartment apartment = context.Apartments.Where(a => a.Code == ApartmentID).Include("Owner").SingleOrDefault();
                if (apartment != null)
                {
                    Owner owner = context.Owners.SingleOrDefault(o=>o.ID == apartment.OwnerID);
                    if (owner != null)
                    {
                        var userIdentity = userManager.FindByIdAsync(owner.ID_Identity);
                        string email = userIdentity.Result.Email;
                        if (email != null)
                        {
                            Mailrequest mailrequest = new Mailrequest();
                            mailrequest.ToEmail = email;
                            mailrequest.Subject = "Rent Request Notification";
                            mailrequest.Body = @"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <style>
        body { font-family: Arial, sans-serif; }
        .container { margin: 20px; padding: 20px; border: 1px solid #ccc; border-radius: 5px; }
        .header { color: #333; }
    </style>
</head>
<body>
    <div class='container'>
        <h1 class='header'>New Rent Request Notification</h1>
        <p>Dear [OwnerName],</p>
        <p>You have received a new request to rent your apartment.</p>
        <p>Apartment Details:</p>
        <ul>
            <li>Location: [ApartmentLocation]</li>
            <li>ID: [ApartmentID]</li>
            <li>Price: [ApartmentPrice] LE</li>
        </ul>
        <p>Requester Details:</p>
        <ul>
            <li>Name: [RequesterName]</li>
            <li>Email: [RequesterEmail]</li>
            <li>Phone: [RequesterPhone]</li>
        </ul>
        <p>Please log in to your dashboard to view more details and respond to the request.</p>
    </div>
</body>
</html>"
                ;

                            Student student = context.Students.Find(StudentID);
                            if (apartment != null && student != null)
                            {
                                mailrequest.Body = mailrequest.Body.Replace("[OwnerName]", apartment.Owner.F_name)
                                 .Replace("[ApartmentLocation]", apartment.Location)
                                 .Replace("[ApartmentID]", apartment.Code.ToString())
                                 .Replace("[ApartmentPrice]", apartment.ApartmentPrice.ToString())
                                 .Replace("[RequesterName]", student.F_name)
                                 .Replace("[RequesterEmail]", student.Email)
                                 .Replace("[RequesterPhone]", student.Phone);
                            }
                            _emailService.SendEmailAsync(mailrequest);
                        }
                        
                    }
                   
                }
                
            }
            catch (Exception ex)
            {

            }






        }

        public void approveRequest(Request request)
        {
            request.Status = "approved";
            Rent rent = new Rent();
            rent.StudentId = request.StudentId;
            rent.ApartmentId = request.ApartmentId;
            rent.Date = DateTime.Now;
            context.Requests.Update(request);
            context.Rents.Add(rent);
            context.SaveChanges();
        }

        public Request approveRequestById(int RequestId)
        {
            Request request = context.Requests.Include("Apartment").SingleOrDefault(r => r.ID == RequestId);

            request.Status = "approved";

            Rent rent = new Rent();
            rent.StudentId = request.StudentId;
            rent.ApartmentId = request.ApartmentId;
            rent.Date = DateTime.Now;
            context.Rents.Add(rent);
            context.SaveChanges();

            Student student = context.Students.SingleOrDefault(i => i.ID == request.StudentId);
            if (student != null)
            {
                Mailrequest mailrequest = new Mailrequest();
                mailrequest.ToEmail = student.Email;
                mailrequest.Subject = "Rent Request Approval Notification";
                mailrequest.Body = @"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <style>
        body { font-family: Arial, sans-serif; }
        .container { margin: 20px; padding: 20px; border: 1px solid #ccc; border-radius: 5px; }
        .header { color: #333; }
        .approval { color: green; }
    </style>
</head>
<body>
    <div class='container'>
        <h1 class='header'>Rent Request Approval</h1>
        <p>Dear [StudentName],</p>
        <p class='approval'>Congratulations! Your request to rent the apartment has been approved.</p>
        <p>Apartment Details:</p>
        <ul>
            <li>Location: [ApartmentLocation]</li>
            <li>ID: [ApartmentID]</li>
            <li>Price: [ApartmentPrice] LE per month</li>
        </ul>
        <p>We look forward to welcoming you to your new home!</p>
        <p>Best regards,</p>

    </div>
</body>
</html>";
                mailrequest.Body = mailrequest.Body.Replace("[StudentName]", student.F_name)
                           .Replace("[ApartmentLocation]", request.Apartment.Location)
                           .Replace("[ApartmentID]", request.Apartment.Code.ToString())
                           .Replace("[ApartmentPrice]", request.Apartment.ApartmentPrice.ToString());
                _emailService.SendEmailAsync(mailrequest);
            }

            return request;
        }

        public async Task<List<Request>> GetRequestsByPerStudent(int studentID)
        {
            return await context.Requests.Where(r => r.StudentId == studentID).ToListAsync();
        }

        public async Task<List<Request>> GetRequestsByPerApartment(int ApartmentID)
        {
            return await context.Requests.Where(r => r.ApartmentId == ApartmentID).ToListAsync();
        }

        public bool ISvalidAparmentID(int Apartment_ID)
        {
            return context.Apartments.Any(a => a.Code == Apartment_ID);
        }

        public bool ISvalidStudentID(int Student_ID)
        {
            return context.Students.Any(s => s.ID == Student_ID);
        }

        public bool ISvalidStudentIDString(string Student_ID)
        {
            var user = userManager.FindByIdAsync(Student_ID);
            if (user == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool IsExistedAparmentID(int Apartment_ID)
        {
            return context.Requests.Any(a => a.ID == Apartment_ID);
        }

        public bool IExistedStudentID(int Student_ID)
        {
            return context.Requests.Any(a => a.ID == Student_ID);
        }

        public Request deleteRequest(int id)
        {
            Request request = context.Requests.Include("Apartment").SingleOrDefault(a => a.ID == id);
            context.Requests.Remove(request);
            context.SaveChanges();
            Student student = context.Students.SingleOrDefault(i => i.ID == request.StudentId);
            if (student != null)
            {
                Mailrequest mailrequest = new Mailrequest();
                mailrequest.ToEmail = student.Email;
                mailrequest.Subject = "Rest Request Rejection Notification";
                mailrequest.Body = "We Apologize Your Request Is Rejected!";
                mailrequest.Body =@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <style>
        body { font-family: Arial, sans-serif; }
        .container { margin: 20px; padding: 20px; border: 1px solid #ccc; border-radius: 5px; }
        .header { color: #333; }
        .rejection { color: red; }
    </style>
</head>
<body>
    <div class='container'>
        <h1 class='header'>Rent Request Decision</h1>
        <p>Dear [StudentName],</p>
        <p class='rejection'>We regret to inform you that your request to rent the apartment at [ApartmentLocation] has been declined.</p>
        <p>We receive a high volume of applications and have to make decisions based on a variety of factors. Unfortunately, your application was not selected for further processing at this time.</p>
        <p>We encourage you to apply for other available apartments in the future, and wish you the best of luck in finding suitable accommodation.</p>
        <p>Thank you for your interest in our property.</p>
        <p>Sincerely,</p>
    </div>
</body>
</html>";
                mailrequest.Body = mailrequest.Body.Replace("[StudentName]", student.F_name)
                           .Replace("[ApartmentLocation]", request.Apartment.Location);


                _emailService.SendEmailAsync(mailrequest);
            }
            return request;
        }

        public bool IExistedRequesttID(int requestID)
        {
            return context.Requests.Any(a => a.ID == requestID);
        }

        public bool IsEmptyTable()
        {
            if (context.Requests.Any())
                return false;
            else
                return true;
        }

        public bool IsRepeatedRequest(int StudentID, int ApartmentID)
        {
            if (context.Requests.Any(s => s.StudentId == StudentID && s.ApartmentId == ApartmentID))
                return true;
            else
                return false;
        }

        public List<Request> GetAllRequests()
        {
            List<Request> requests = context.Requests.ToList();
            return requests;
        }
        public List<Request> GetAllRequestsRelatedToUserApartments(string userID)
        {
            //Get ApartmentsIDs By UserID
            Owner owner = context.Owners.SingleOrDefault(o => o.ID_Identity == userID);
            if (owner == null)
            {
                return new List<Request>();
            }
            else
            {
                int ownerID = owner.ID;
                List<Request> requests = (from request in context.Requests
                                          join apartment in context.Apartments
                                          on request.ApartmentId equals apartment.Code
                                          where apartment.OwnerID == ownerID
                                          select request).ToList();

                return requests;


                //int ownerID = owner.ID;
                //List<int> ApartmentsIDs = context.Apartments.Where(a => a.OwnerID == ownerID).Select(a => a.Code).ToList();

                //List<Request> requests = context.Requests.Where(r => ApartmentsIDs.Contains(r.ApartmentId)).ToList();
                //return requests;
            }

        }

        public List<Request> GetAllApprovedRequests()
        {
            List<Request> requests = context.Requests.Where(r => r.Status == "approved").ToList();
            return requests;
        }

        public async Task<List<Request>> GetAllPendingRequests()
        {
            List<Request> requests = await context.Requests.Where(r => r.Status == "pending").ToListAsync();
            return requests;
        }
        public async Task<List<Request>> GetAllApprovedRequestsPerStudent(string StudentID)
        {
            int stID = await GetStudentID(StudentID);
            List<Request> requests = context.Requests.Include(ap => ap.Apartment).Where(r => r.Status == "approved" && r.StudentId == stID).ToList();
            return requests;
        }

        public async Task<List<Request>> GetAllPendingRequestsPerStudent(string StudentID)
        {
            int stID = await GetStudentID(StudentID);
            List<Request> requests = context.Requests.Include(ap => ap.Apartment).Where(r => r.Status == "pending" && r.StudentId == stID).ToList();
            return requests;
        }

        public bool IsApprovedRequest(int RequestID)
        {
            if (context.Requests.SingleOrDefault(r => r.ID == RequestID).Status == "approved")
                return true;
            else return false;
        }

        public bool IsRentedApartment(int ApartmentID)
        {
            if (context.Rents.Any(r => r.ApartmentId == ApartmentID))
                return true;
            else
                return false;
        }


        public async Task<int> GetStudentID(string UserIDFromIdentity)
        {
            var user = await context.Students.SingleOrDefaultAsync(s => s.ID_Identity == UserIDFromIdentity);
            if (user == null)
            {
                return 0;
            }
            else
            {
                int studentID = user.ID;
                return studentID;
            }

        }

        public int GetOwnerID(string UserIDFromIdentity)
        {
            var user = context.Owners.SingleOrDefault(s => s.ID_Identity == UserIDFromIdentity);
            int OwnerID = user.ID;
            return OwnerID;
        }

        public string GetUserEmail(int RequestID)
        {
            try
            {
                Request request = context.Requests.SingleOrDefault(r => r.ID == RequestID);
                Student student = context.Students.SingleOrDefault(i => i.ID == request.StudentId);
                string email = student.Email;
                return email;
            }
            catch (Exception ex)
            {
                return "ahmed.abdelatty3030@gmail.com";
            }

        }
        public string GetUserEmailByUserID(int UserID)
        {
            try
            {
                Student student = context.Students.SingleOrDefault(i => i.ID == UserID);
                string email = student.Email;
                return email;
            }
            catch (Exception ex)
            {
                return "ahmed.abdelatty3030@gmail.com";
            }

        }

        public int SendMail(int RequestID)
        {
            try
            {
                string email = GetUserEmail(RequestID);
                Mailrequest mailrequest = new Mailrequest();
                mailrequest.ToEmail = email;
                mailrequest.Subject = "Aprrove Request";
                mailrequest.Body = "Your Request Approved Sucessfully!";
                _emailService.SendEmailAsync(mailrequest);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

       
    }
}
