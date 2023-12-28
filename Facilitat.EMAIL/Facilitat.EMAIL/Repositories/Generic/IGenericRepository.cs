using System.Collections.Generic;
using System.Threading.Tasks;

namespace Facilitat.EMAIL.Repositories.Generic
{
    public interface IGenericRepository<T>
    {
        Task<T> GetById(int id);
        Task<IEnumerable<T>> GetAll();
        Task<bool> Add(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(T entity);
    }
}
