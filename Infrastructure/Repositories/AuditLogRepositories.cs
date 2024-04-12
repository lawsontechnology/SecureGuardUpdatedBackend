using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Visitor_Management_System.Core.Application.Interface.Repositories;
using Visitor_Management_System.Core.Domain.Entities;
using Visitor_Management_System.Infrastructure.Context;

namespace Visitor_Management_System.Infrastructure.Repositories
{
    public class AuditLogRepositories : BaseRepositories<AuditLog>, IAuditLogRepo
    {
        public AuditLogRepositories(VisitorManagementSystem context)
        {
           _context = context;
        }
        public async Task<AuditLog> Get(string id)
        {
            var auditLog = await _context.AuditLogs
            .SingleOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
            return auditLog;
        }

        public async Task<AuditLog> Get(Expression<Func<AuditLog, bool>> predicate)
        {
            var auditLog = await _context.AuditLogs
                .Where(x => x.IsDeleted == false)
            .FirstOrDefaultAsync(predicate);
            return auditLog;
        }

        public async Task<ICollection<AuditLog >> GetAll(Paging paging)
        {
            return await _context.AuditLogs
             .Skip((paging.PageNumber - 1) * paging.PageSize)
             .Take(paging.PageSize)
             .Where(x => x.IsDeleted == false)
             .ToListAsync();
        }

        public async Task<(ICollection<AuditLog> Data, int TotalCount)> GetAllPagination(Paging paging)
        {
            var query = _context.AuditLogs
           .Where(x => x.IsDeleted == false);
             

            var totalCount = await query.CountAsync();

                var data = await query
                 .Skip((paging.PageNumber - 1) * paging.PageSize)
                 .Take(paging.PageSize)
                    .ToListAsync();

                return (data, totalCount);
            
        }

    }
}
