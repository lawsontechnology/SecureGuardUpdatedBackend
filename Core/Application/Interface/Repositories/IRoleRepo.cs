using System.Linq.Expressions;
using Visitor_Management_System.Core.Domain.Entities;
using Visitor_Management_System.Infrastructure;

namespace Visitor_Management_System.Core.Application.Interface.Repositories
{
    public interface IRoleRepo : IBaseResponse<Role>
    {
        Task<Role> Get(string id);
        Task<Role> Get(Expression<Func<Role, bool>> predicate);
        Task<ICollection<Role>> GetAll(Paging paging);
    }
}
