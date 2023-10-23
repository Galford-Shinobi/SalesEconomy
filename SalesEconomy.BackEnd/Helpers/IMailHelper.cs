using SalesEconomy.Shared.Responses;

namespace SalesEconomy.BackEnd.Helpers
{
    public interface IMailHelper
    {
        Response<string> SendMail(string toName, string toEmail, string subject, string body);
    }
}
