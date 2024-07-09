using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Waseet.Helper;
using Waseet.Models;
using Waseet.Service;

namespace Waseet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        private readonly IEmailService _emailService;
        private readonly RentalContext _rentalContext;

        public TestController(IEmailService emailService, RentalContext rentalContext)
        {
            _emailService = emailService;
            _rentalContext = rentalContext;

        }



        [HttpGet("triggerEmailFromAnotherAction")]
        public IActionResult TriggerEmailFromAnotherAction()
        {
            var Request = _rentalContext.Requests.FirstOrDefault();

            string toEmail = "en.mostafa.hamada@gmail.com";
            string sub = "test subject ";
            string bodey = @"<!DOCTYPE html>
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
            <li>Price: $[ApartmentPrice]</li>
        </ul>
        <p>Please contact us to finalize the rental agreement and move-in details.</p>
        <p>Thank you for choosing us!</p>
        <div class=""footer"">
            <p>Your Company Name</p>
        </div>
    </div>
</body>
</html>

";

            

            // Now htmlBody contains the updated HTML with actual values


            Mailrequest mailrequest = new Mailrequest() { ToEmail = toEmail, Subject = sub, Body = bodey };
             _emailService.SendEmail(mailrequest);

            return Ok();
        }





    }
}
