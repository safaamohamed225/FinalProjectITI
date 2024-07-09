using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Waseet.DTO;
using Waseet.Helper;
using Waseet.Service;

namespace Waseet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public MailController(IEmailService emailService)
        {
            _emailService = emailService;
        }



        [HttpPost("SendMail")]
        public async Task<IActionResult> SendMail(string toemail, string subject, string body)
        {
            try
            {
                Mailrequest mailrequest = new Mailrequest();
                mailrequest.ToEmail = toemail;
                mailrequest.Subject = subject;
                mailrequest.Body = body;
                await _emailService.SendEmailAsync(mailrequest);
                return Ok(mailrequest);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            

        }


        //public string Email { get; set; }
        //public string Name { get; set; }
        //public string Message { get; set; }
        //public string Subject { get; set; }
        //public string Phone { get; set; }



        [HttpPost("ContactUS")]
        public async Task<IActionResult> ContactUS(ContactUSDTO contactUSDTO)
        { 
            try
            {
                Mailrequest mailrequest = new Mailrequest();
                mailrequest.ToEmail = "bassamserag055@gmail.com";
                mailrequest.Subject = contactUSDTO.Subject;
                //mailrequest.Body = $"Name : {contactUSDTO.Name} \n E-Mail : {contactUSDTO.Email}\n Phone : {contactUSDTO.Phone} \n Message : {contactUSDTO.Message}\n ";
                mailrequest.Body = $"Name: {contactUSDTO.Name}<br/>E-Mail: {contactUSDTO.Email}<br/>Phone: {contactUSDTO.Phone}<br/>Message: {contactUSDTO.Message}<br/>";
                await _emailService.SendEmailAsync(mailrequest);
                return Ok(mailrequest);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);  
            }
            

        }
    }
}
