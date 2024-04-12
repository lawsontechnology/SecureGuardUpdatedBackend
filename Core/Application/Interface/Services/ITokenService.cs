using Visitor_Management_System.Core.Application.DTOs;
using Visitor_Management_System.Core.Domain.Entities;

namespace Visitor_Management_System.Core.Application.Interface.Services
{
    public interface ITokenService
    {
        Task<Token> GetById(string id);
         Task<string> GenerateApproveToken();
        Task<BaseResponse<TokenDto>> ValidateAndRemoveToken(string token);

    }
}
