using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Visitor_Management_System.Core.Application.Interface.Repositories;
using Visitor_Management_System.Core.Domain.Entities;
using Visitor_Management_System.Core.Domain.Enum;
using Visitor_Management_System.Infrastructure.Context;

namespace Visitor_Management_System.Infrastructure.Repositories
{
    public class VisitRepositories : BaseRepositories<Visit>, IVisitRepo
    {
        public VisitRepositories(VisitorManagementSystem context)
        {
            _context = context;  
        }
        public async Task<Visit> Get(string id)
        {
            return await _context.Visits
            .Include(x => x.User)
            .Include(x => x.Visitor)
            .SingleOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
            
        }

        public async Task<Visit> Get(Expression<Func<Visit, bool>> predicate)
        {
            var visit = await _context.Visits
             .Include(x => x.User)
             .Include(x => x.Visitor)
             .Where(x => x.IsDeleted == false)
             .SingleOrDefaultAsync(predicate);
            return visit;
        }

        public async Task<ICollection<Visit>> GetAll(Paging paging)
        {
            return await _context.Visits
            .Include(x => x.User)
            .Include(x => x.Visitor)
            .Skip((paging.PageNumber - 1) * paging.PageSize)
            .Take(paging.PageSize)
            .Where(x => x.IsDeleted == false)
            .ToListAsync();
        }
        public async Task<ICollection<Visit>> GetAll(Expression<Func<Visit, bool>> predicate, Paging paging)
        {
            return await _context.Visits
                .Include(x => x.User)
                .Include(x => x.Visitor)
                .Skip((paging.PageNumber - 1) * paging.PageSize)
                .Take(paging.PageSize)
                .Where(x => !x.IsDeleted)
                .Where(predicate)
                .ToListAsync();
        }
        
    }
}
