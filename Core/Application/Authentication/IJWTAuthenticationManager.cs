using Visitor_Management_System.Core.Application.DTOs;

namespace Visitor_Management_System.Core.Application.Authentication
{
    public interface IJWTAuthenticationManager
    {
        public string GenerateToken(UserDto user);
    }
}
