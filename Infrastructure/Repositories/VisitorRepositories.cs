using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Visitor_Management_System.Core.Application.Interface.Repositories;
using Visitor_Management_System.Core.Domain.Entities;
using Visitor_Management_System.Infrastructure.Context;

namespace Visitor_Management_System.Infrastructure.Repositories
{
    public class VisitorRepositories : BaseRepositories<Visitor>, IVisitorRepo
    {
        public VisitorRepositories(VisitorManagementSystem context)
        {
            _context = context;
        }
        public Task<Visitor> Get(string id)
        {
            var visitor = _context.Visitors
            .Include(x => x.Visits)
            .ThenInclude(x => x.User)
            .SingleOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
            return visitor;
        }

        public async Task<Visitor> Get(Expression<Func<Visitor, bool>> predicate)
        {
            var visitor = await _context.Visitors
            .Include(x => x.Visits)
            .ThenInclude(x => x.User)
            .Where(x => x.IsDeleted == false)
            .SingleOrDefaultAsync(predicate);
            return visitor;
        }

        public async Task<ICollection<Visitor>> GetAll(Paging paging)
        {
            return await _context.Visitors
            .Include(x => x.Visits)
            .ThenInclude(x => x.User)
            .Skip((paging.PageNumber - 1) * paging.PageSize)
            .Take(paging.PageSize)
            .Where(x => x.IsDeleted == false)
            .ToListAsync();
        }

        public async Task<ICollection<Visitor>> GetAll(Expression<Func<Visitor, bool>> predicate, Paging paging)
        {
            return await _context.Visitors
                .Include(x => x.Visits)
                .ThenInclude(x => x.User)
                .Skip((paging.PageNumber - 1) * paging.PageSize)
                .Take(paging.PageSize)
                /*.Where(x => !x.IsDeleted)*/
                .Where(predicate)
                .ToListAsync();
        }
    }
}
