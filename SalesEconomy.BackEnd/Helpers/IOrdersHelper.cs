using SalesEconomy.Shared.Responses;

namespace SalesEconomy.BackEnd.Helpers
{
    public interface IOrdersHelper
    {
        Task<Response<bool>> ProcessOrderAsync(string email, string remarks);
    }
}
