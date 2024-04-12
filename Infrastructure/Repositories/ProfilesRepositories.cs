using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Visitor_Management_System.Core.Application.Interface.Repositories;
using Visitor_Management_System.Core.Domain.Entities;
using Visitor_Management_System.Infrastructure.Context;

namespace Visitor_Management_System.Infrastructure.Repositories
{
    public class ProfilesRepositories : BaseRepositories<Profile>, IProfileRepo
    {
        public ProfilesRepositories(VisitorManagementSystem context)
        {
            _context = context;
        }
        public async Task<Profile> Get(string id)
        {
            var profile = await _context.Profiles
            .Include(x => x.User)
            .SingleOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
            return profile;
        }

        public async Task<Profile> Get(Expression<Func<Profile, bool>> predicate)
        {
            var profile = await _context.Profiles
            .Include(x => x.User)
            .Where(x => x.IsDeleted == false)
            .SingleOrDefaultAsync(predicate);
            return profile;
        }

        public async Task<ICollection<Profile>> GetAll()
        {
            return await _context.Profiles
            .Include(x => x.User)
            .Where(x => x.IsDeleted == false)
            .ToListAsync();
        }
    }
}
