using System.Linq.Expressions;
using Visitor_Management_System.Core.Domain.Entities;
using Visitor_Management_System.Core.Application.DTOs;
using Visitor_Management_System.Infrastructure;

namespace Visitor_Management_System.Core.Application.Interface.Repositories
{
    public interface IAuditLogRepo : IBaseResponse<AuditLog>
    {
        Task<AuditLog> Get(string id);
        Task<AuditLog> Get(Expression<Func<AuditLog, bool>> predicate);
        Task<ICollection<AuditLog>> GetAll(Paging paging);
        Task<(ICollection<AuditLog> Data, int TotalCount)> GetAllPagination(Paging paging);
        

    }
}
