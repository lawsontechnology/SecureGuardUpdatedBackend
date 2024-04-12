using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Visitor_Management_System.Core.Application.Interface.Repositories;
using Visitor_Management_System.Core.Domain.Entities;
using Visitor_Management_System.Infrastructure.Context;

namespace Visitor_Management_System.Infrastructure.Repositories
{
    public class RoleRepositories : BaseRepositories<Role>, IRoleRepo
    {
        public RoleRepositories(VisitorManagementSystem context)
        {
            _context = context;
        }
        public async Task<Role> Get(string id)
        {
            var getRole = await _context.Roles
         .Include(n => n.UserRoles)
         .ThenInclude(n => n.User)
         .SingleOrDefaultAsync(j => j.Id == id && j.IsDeleted == false);
            return getRole;
        }

        public async Task<Role> Get(Expression<Func<Role, bool>> predicate)
        {
            var getRole = await _context.Roles
            .Include(n => n.UserRoles)
            .ThenInclude(n => n.User)
            .Where(x => x.IsDeleted == false)
            .SingleOrDefaultAsync(predicate);
            return getRole;
        }

        public async Task<ICollection<Role>> GetAll(Paging paging)
        {
            return await _context.Roles
            .Include(n => n.UserRoles)
            .ThenInclude(n => n.User)
            .Skip((paging.PageNumber - 1) * paging.PageSize)
            .Take(paging.PageSize)
            .Where(x => x.IsDeleted == false)
            .ToListAsync();
        }
    }
}
