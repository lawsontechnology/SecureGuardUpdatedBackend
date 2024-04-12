using System;
using System.Threading.Tasks;
using Visitor_Management_System.Core.Application.DTOs;
using Visitor_Management_System.Core.Application.Interface.Repositories;
using Visitor_Management_System.Core.Application.Interface.Services;
using Visitor_Management_System.Core.Domain.Entities;

namespace Visitor_Management_System.Infrastructure
{
    public class TokenService : ITokenService
    {
        private readonly IToken _tokenRepository;

        public TokenService(IToken tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }



        public async Task<string> GenerateApproveToken()
        {
            string tokenValue = Guid.NewGuid().ToString().Substring(0, 6);
            var token = new Token()
            {
                Tokens = tokenValue,
                DateCreated = DateTime.Now,
            };

            await _tokenRepository.CreateAsync(token);
            await _tokenRepository.SaveAsync();

            return token.Tokens; 
        }

        public async Task<Token> GetById(string id)
        {
            return await _tokenRepository.Get(id);
        }

        public async Task<BaseResponse<TokenDto>>ValidateAndRemoveToken(string token)
        {
            
            var isValid = _tokenRepository.Get(t => t.Tokens == token);
            if (isValid != null)
            {
                 _tokenRepository.RemoveToken(token);
                await  _tokenRepository.SaveAsync();
                return new BaseResponse<TokenDto>
                {
                    Message = "Successful remove Token",
                    Status = true,
                };
            }

            return new BaseResponse<TokenDto>
            {
                Status = false,
                Message = " Invalid Tokens"
            };
        }
    }
}
