using System.Linq.Expressions;
using Visitor_Management_System.Core.Domain.Entities;
using Visitor_Management_System.Infrastructure;

namespace Visitor_Management_System.Core.Application.Interface.Repositories
{
    public interface IUserRepo : IBaseResponse<User>
    {
        Task<User> Get(string id);
        Task<User> Get(Expression<Func<User, bool>> predicate);
        Task<ICollection<User>> GetAll(Paging paging);
    }
}
