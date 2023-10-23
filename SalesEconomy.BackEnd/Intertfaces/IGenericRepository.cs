using SalesEconomy.Shared.Entities;
using SalesEconomy.Shared.Responses;

namespace SalesEconomy.BackEnd.Intertfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetAsync(int id);

        Task<IEnumerable<T>> GetAsync();

        Task<Response<T>> AddAsync(T entity);

        Task<Response<T>> DeleteAsync(int id);

        Task<Response<T>> UpdateAsync(T entity);

        Task<Country> GetCountryAsync(int id);

        Task<State> GetStateAsync(int id);
    }
}
