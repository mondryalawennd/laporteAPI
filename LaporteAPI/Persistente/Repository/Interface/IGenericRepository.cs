using System.Linq.Expressions;

namespace LaporteAPI.Persistente.Repository.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> Add(T entity);
        Task<T> GetEntityById(int id);
        Task<IEnumerable<T>> List();
        Task Update(T entity);
        Task Delete(T entity);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
        Task<T?> BuscarPorCampo(Expression<Func<T, bool>> predicate);
    }
}
