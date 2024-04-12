using System.Linq.Expressions;
using Visitor_Management_System.Core.Domain.Entities;

namespace Visitor_Management_System.Core.Application.Interface.Repositories
{
    public interface IProfileRepo : IBaseResponse<Profile>
    {
        Task<Profile> Get(string id);
        Task<Profile> Get(Expression<Func<Profile, bool>> predicate);
        Task<ICollection<Profile>> GetAll();
    }
}
