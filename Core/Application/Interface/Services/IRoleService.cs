using Visitor_Management_System.Core.Application.DTOs;
using Visitor_Management_System.Infrastructure;

namespace Visitor_Management_System.Core.Application.Interface.Services
{
    public interface IRoleService
    {
        Task<BaseResponse<RoleDto>> Register(RoleRequestModel model, string userEmail);
        Task<BaseResponse<RoleDto>> Get(string id, string userEmail);
        Task<BaseResponse<RoleDto>> GetUser(string RoleName, string userEmail);
        Task<BaseResponse<ICollection<RoleDto>>> GetAll(string userEmail, Paging paging);
    }
}
