using Humanizer;
using Microsoft.AspNetCore.Identity;
using Waseet.Helper;
using Waseet.Models;
using Waseet.Service;

namespace Waseet.Repsitory.ApprovalForPostRepo
{
    public class ApprovalForPostRepo : IApprovalForPost
    {
        RentalContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmailService emailService;
        public ApprovalForPostRepo(RentalContext context , UserManager<ApplicationUser> _userManager, IEmailService _emailService)
        {
            this.context = context;
            userManager = _userManager;
            emailService = _emailService;
        }
       

        public void ApproveApartmentForPost(int apart_ID, int admin_ID)
        {
            Apartment apartment = context.Apartments.Find(apart_ID);
            if (apartment != null)
            {
                
                try
                {
                    apartment.Status = "approved";
                    context.ApprovalStatuses.Add(new ApprovalStatus { AdminId = admin_ID, ApartmentId = apart_ID });
                    context.SaveChanges();

                }
                catch (Exception ex) 
                { 
                    
                }

                try
                {                   
                    if (apartment != null)
                    {
                        Owner owner = context.Owners.SingleOrDefault(o => o.ID == apartment.OwnerID);
                        if (owner != null)
                        {
                            var userIdentity = userManager.FindByIdAsync(owner.ID_Identity);
                            string email = userIdentity.Result.Email;
                            if (email != null)
                            {
                                Mailrequest mailrequest = new Mailrequest();
                                mailrequest.ToEmail = email;
                                mailrequest.Subject = "Apartment Approval";
                                mailrequest.Body = @"<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 20px;
            padding: 0;
            color: #333;
        }
        .container {
            background-color: #f4f4f4;
            padding: 20px;
            border-radius: 5px;
        }
        .header {
            background: #007bff;
            color: #ffffff;
            padding: 10px;
            text-align: center;
            border-radius: 5px 5px 0 0;
        }
        .footer {
            background: #007bff;
            color: #ffffff;
            padding: 10px;
            text-align: center;
            border-radius: 0 0 5px 5px;
            margin-top: 20px;
        }
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""header"">
            <h2>Apartment Approval Notification</h2>
        </div>
        <p>Dear [RecipientName],</p>
        <p>We are pleased to inform you that your request for the following apartment has been approved:</p>
        <ul>
            <li>Apartment ID: [ApartmentID]</li>
            <li>Location: [ApartmentLocation]</li>
            <li>Price: [ApartmentPrice] LE </li>
        </ul>
        
        <p>Thank you for choosing us!</p>
        <div class=""footer"">
            <p>Waseet</p>
        </div>
    </div>
</body>
</html>

";


                                mailrequest.Body = mailrequest.Body.Replace("[RecipientName]", owner.F_name)
                               .Replace("[ApartmentID]", apartment.Code.ToString())
                               .Replace("[ApartmentLocation]", apartment.Location)
                               .Replace("[ApartmentPrice]", apartment.ApartmentPrice.ToString());


                                emailService.SendEmailAsync(mailrequest);
                            }

                        }

                    }

                }
                catch (Exception ex)
                {

                }

            }
            
        }



        public void RejectApartmentForPost(int apart_ID, int admin_ID , string RejectionMsg)
        {
            Apartment apartment = context.Apartments.Find(apart_ID);
            if (apartment != null)
            {

                try
                {
                    apartment.Status = "rejected";
                    context.ApprovalStatuses.Add(new ApprovalStatus { AdminId = admin_ID, ApartmentId = apart_ID });
                    context.SaveChanges();

                }
                catch (Exception ex)
                {

                }

                try
                {
                    if (apartment != null)
                    {
                        Owner owner = context.Owners.SingleOrDefault(o => o.ID == apartment.OwnerID);
                        if (owner != null)
                        {
                            var userIdentity = userManager.FindByIdAsync(owner.ID_Identity);
                            string email = userIdentity.Result.Email;
                            if (email != null)
                            {
                                Mailrequest mailrequest = new Mailrequest();
                                mailrequest.ToEmail = email;
                                mailrequest.Subject = $"Apartment Rejection -- Reason : {RejectionMsg}";
                                mailrequest.Body = @"<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 20px;
            padding: 0;
            color: #333;
        }
        .container {
            background-color: #f4f4f4;
            padding: 20px;
            border-radius: 5px;
        }
        .header {
            background: #007bff;
            color: #ffffff;
            padding: 10px;
            text-align: center;
            border-radius: 5px 5px 0 0;
        }
        .footer {
            background: #007bff;
            color: #ffffff;
            padding: 10px;
            text-align: center;
            border-radius: 0 0 5px 5px;
            margin-top: 20px;
        }
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""header"">
            <h2>Apartment Rejection Notification!</h2>
        </div>
        <p>Dear [RecipientName],</p>
        <p>We Apologize to inform you that your request for the following apartment has been Rejected Due to : $[RejectionReasons]</p>
        <ul>
            <li>Apartment ID: [ApartmentID]</li>
            <li>Location: [ApartmentLocation]</li>
            <li>Price: [ApartmentPrice] LE</li>
            <li>Rejection Reasons: [RejectionReasons]</li>
        </ul>
        
        <p>Thank you for choosing us!</p>
        <div class=""footer"">
            <p>Waseet</p>
        </div>
    </div>
</body>
</html>

";
                                mailrequest.Body = mailrequest.Body.Replace("[RecipientName]", owner.F_name)
                               .Replace("[ApartmentID]", apartment.Code.ToString())
                               .Replace("[ApartmentLocation]", apartment.Location)
                               .Replace("[ApartmentPrice]", apartment.ApartmentPrice.ToString())
                               .Replace("[RejectionReasons]", RejectionMsg); ;


                                emailService.SendEmailAsync(mailrequest);
                            }

                        }

                    }

                }
                catch (Exception ex)
                {

                }

            }

        }

        public int GetAdmin_Who_ApprovedApartment(int Apartment_ID)
        {
            if( context.Apartments.Any(m=>m.Code == Apartment_ID))
            {
                if (context.ApprovalStatuses.Any(m => m.ApartmentId == Apartment_ID))
                {
                    return context.ApprovalStatuses.Where(n => n.ApartmentId == Apartment_ID)
                                  .Select(a => a.AdminId).FirstOrDefault();
                }
                return 0;
            }
            return 0;
            //{
            //    int adminID = context.ApprovalStatuses.Where(n => n.ApartmentId == Apartment_ID)
            //                        .Select(a => a.AdminId).FirstOrDefault();

            //    return adminID;
            //}
            //return 0;    
        }

        public List<int> GetApprovedAppartmentsByAdmin(int admin_ID)
        {
            return context.ApprovalStatuses.Where(n=>n.AdminId == admin_ID).Select(a => a.ApartmentId).ToList();
        }

        public bool IsApprovedApartment(int Apartment_ID)
        {
            return context.ApprovalStatuses.Any(n => n.ApartmentId == Apartment_ID);
        }

        public bool ISvalidAdminID(int Admin_ID)
        {
            return context.Admins.Any(n=>n.ID == Admin_ID);
        }

        public bool ISvalidAparmentID(int Apartment_ID)
        {
            return context.Apartments.Any(n=>n.Code == Apartment_ID);
        }


    }
}
