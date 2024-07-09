using Waseet.Helper;

namespace Waseet.Service
{
    public interface IEmailService
    {

        Task SendEmailAsync(Mailrequest mailrequest);

        void SendEmail(Mailrequest mailrequest); 


    }


}
