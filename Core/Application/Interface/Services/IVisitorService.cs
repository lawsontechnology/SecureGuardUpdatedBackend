using Visitor_Management_System.Core.Application.DTOs;
using Visitor_Management_System.Core.Domain.Enum;
using Visitor_Management_System.Infrastructure;

namespace Visitor_Management_System.Core.Application.Interface.Services
{
    public interface IVisitorService
    {
        Task<BaseResponse<VisitorDto>> Register(VisitorRequestModel model, VisitRequestModel visitModel);
        Task<BaseResponse<VisitorDto>> GetById(string id);
        Task<BaseResponse<VisitorDto>> GetByEmail(string email, string userEmail, string userRole);
        Task<BaseResponse<VisitorDto>> Delete(string Id,string userEmail, string userRole);
        Task<BaseResponse<ICollection<VisitDto>>> GetVisitByHostEmail(string hostEmail,string userEmail, string userRole, Paging paging);
        Task<BaseResponse<ICollection<VisitorDto>>> GetByHostEmail(string hostEmail, string userEmail, string userRole, Paging paging);
        Task<BaseResponse<ICollection<VisitorDto>>> GetAll(string userEmail,string userRole, Paging paging);
        Task<BaseResponse<VisitorDto>> GetVisit(string visitId);
        Task<BaseResponse<VisitDto>> ApproveRequest(string VisitId);
        
        Task<BaseResponse<VisitDto>> DeniedRequest(string VisitId);
        
    }
}
