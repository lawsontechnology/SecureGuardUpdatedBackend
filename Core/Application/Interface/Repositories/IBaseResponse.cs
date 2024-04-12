using System.Linq.Expressions;

namespace Visitor_Management_System.Core.Application.Interface.Repositories
{
    public interface IBaseResponse<T>
    {
        Task<T> CreateAsync(T entity);
        T Update(T entity);
        bool Check(Expression<Func<T, bool>> predicate);
        Task<int> SaveAsync();
        Task<ICollection<T>> AddRangeAsync(ICollection<T> entities);
    }
}
