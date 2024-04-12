using System.Linq.Expressions;
using Visitor_Management_System.Core.Domain.Entities;

namespace Visitor_Management_System.Core.Application.Interface.Repositories
{
    public interface IAddressRepo : IBaseResponse<Address>
    {
        Task<Address> Get(string id);
        Task<Address> Get(Expression<Func<Address, bool>> predicate);
        Task<ICollection<Address>> GetAll();
    }
}
