using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Visitor_Management_System.Core.Application.Interface.Repositories;
using Visitor_Management_System.Core.Domain.Entities;
using Visitor_Management_System.Infrastructure.Context;

namespace Visitor_Management_System.Infrastructure.Repositories
{
    public class UserRepositories : BaseRepositories<User>, IUserRepo
    {
        public UserRepositories(VisitorManagementSystem context)
        {
            _context = context;
        }
        public async Task<User> Get(string id)
        {
            var getUser = await _context.Users
            .Include(x => x.Profile)
           .Include(x => x.UserRoles)
           .ThenInclude(x => x.Role)
           .Include(x => x.Visits)
           .ThenInclude(x => x.Visitor)
           .SingleOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
            return getUser;
        }

        public async Task<User> Get(Expression<Func<User, bool>> predicate)
        {
            var getUser = await _context.Users
           .Include(x => x.Profile)
           .Include(x => x.UserRoles)
           .ThenInclude(x => x.Role)
           .Include(x => x.Visits)
           .ThenInclude(x => x.Visitor)
           .Where(x => x.IsDeleted == false)
           .SingleOrDefaultAsync(predicate);
            return getUser;
        }

        public async Task<ICollection<User>> GetAll(Paging paging)
        {
            return await _context.Users
            .Include(x => x.Profile)
           .Include(x => x.UserRoles)
           .ThenInclude(x => x.Role)
           .Include(x => x.Visits)
           .ThenInclude(x => x.Visitor)
           .Skip((paging.PageNumber - 1) * paging.PageSize)
           .Take(paging.PageSize)
           .Where(x => x.IsDeleted == false)
           .ToListAsync();

        }
    }
}
