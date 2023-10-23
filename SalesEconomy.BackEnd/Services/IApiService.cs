using SalesEconomy.Shared.Responses;

namespace SalesEconomy.BackEnd.Services
{
    public interface IApiService
    {
        Task<Response<T>> GetAsync<T>(string servicePrefix, string controller);
    }
}
