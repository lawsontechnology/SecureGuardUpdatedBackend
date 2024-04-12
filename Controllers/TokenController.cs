

using Microsoft.AspNetCore.Mvc;
using Visitor_Management_System.Core.Application.Interface.Repositories;
using Visitor_Management_System.Core.Application.Interface.Services;

namespace Visitor_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : Controller
    {
        private readonly ITokenService _token;

        public TokenController(ITokenService token)
        {
            _token = token;
        }

        [HttpPost("Validation")]
        public async Task <IActionResult> tokenValidation([FromQuery] string token)
        {
            var checkToken = await _token.ValidateAndRemoveToken(token);
            if (checkToken.Status)
            {
                return Ok(checkToken);
            }
            else
            {
                return BadRequest(checkToken);
            }
        }
    }
}
