
using Visitor_Management_System.Core.Application.DTOs;
using Visitor_Management_System.Infrastructure;

namespace Visitor_Management_System.Core.Application.Interface.Services
{
    public interface IAuditLogService
    {
        Task<BaseResponse<AuditLogDto>> Get(string UserId,string userEmail);
        Task<BaseResponse<ICollection<AuditLogDto>>> GetAll(string userEmail, Paging paging);
        Task<BaseResponse<AuditLogDto>> Delete(string Id, string userEmail);
        
    }
}
