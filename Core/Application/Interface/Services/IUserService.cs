using Visitor_Management_System.Core.Application.DTOs;
using Visitor_Management_System.Infrastructure;

namespace Visitor_Management_System.Core.Application.Interface.Services
{
    public interface IUserService
    {
        Task<BaseResponse<UserDto>> Login(LoginRequestModel model);
        Task<BaseResponse<UserDto>> RegisterAdmin(AdminProfileRequestModel model);
        /*Task<BaseResponse<ICollection<UserDto>>> SaveUsersToDatabase(List<UserDto> users, List<ProfileDto> profiles);*/
        Task<BaseResponse<UserDto>> Update(string id, UpdateProfileRequestModel model, string userRole);
        Task<BaseResponse<UserDto>> GetById(string id);
        Task<BaseResponse<UserDto>> Get(string email, string userEmail);
        Task<BaseResponse<ICollection<UserDto>>> GetAllSecurity(string userEmail, Paging paging);
        Task<BaseResponse<ICollection<UserDto>>> GetAllHost(string userEmail, Paging paging);
        Task<BaseResponse<ICollection<UserDto>>> GetAll(string userEmail, Paging paging);
        Task<BaseResponse<UserDto>> UserRegister(RegisterRequestModel model, string userEmail);
        Task<BaseResponse<UserDto>> Delete(string Id, string userEmail);
    }
}
