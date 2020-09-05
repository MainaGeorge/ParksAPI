using System.Collections.Generic;
using System.Threading.Tasks;

namespace NationalParksProject.Services.IRepository
{
    public interface IRepository<T>  where T : class
    {
        Task<T> GetById(string url, int id);

        Task<IEnumerable<T>> GetAll(string url);

        Task<bool> CreateAsync(string url, T createObject);

        Task<bool> DeleteAsync(string url, int id);

        Task<bool> UpdateAsync(string url, int id, T updateObject);

    }
}
