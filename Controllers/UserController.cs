using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Visitor_Management_System.Core.Application.Authentication;
using Visitor_Management_System.Core.Application.DTOs;
using Visitor_Management_System.Core.Application.Interface.Services;
using Visitor_Management_System.Infrastructure;

namespace Visitor_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IJWTAuthenticationManager _jwtAuthenticationManager;

        public UserController(IUserService userService, IJWTAuthenticationManager jwtAuthenticationManager)
        {
            _userService = userService;
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }

        [HttpPost("Register/Admin")]
        public async Task<IActionResult> RegisterAdmin([FromForm] AdminProfileRequestModel request)
        {
            var user = await _userService.RegisterAdmin(request);
            return user.Status ? Ok(user) : NotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Register/User")]
        public async Task<IActionResult> RegisterUser([FromForm] RegisterRequestModel request)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userService.UserRegister(request,userEmail);
            return user.Status ? Ok(user) : NotFound();
        }

        [Authorize(Roles = "Host,Security")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromForm] UpdateProfileRequestModel request)
        {
            var userRole = User.FindFirst("RoleName")?.Value;
            var user = await _userService.Update(id, request,userRole);
            return user.Status ? Ok(user) : NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var user = await _userService.GetById(id);
            return user.Status ? Ok(user) : BadRequest(user.Message);
        }

        [Authorize (Roles ="Admin") ]
        [HttpGet("All/Securities")]
        public async Task<IActionResult> GetAllSecurities([FromQuery] Paging paging)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userService.GetAllSecurity(userEmail,paging);
            return user.Status ? Ok(user.Data) : BadRequest(user.Message);
        }
         
        [Authorize(Roles = "Admin")]
        [HttpGet("All/Hosts")]
        public async Task<IActionResult> GetAllHost([FromQuery] Paging paging)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userService.GetAllHost(userEmail,paging);
            return user.Status ? Ok(user.Data) : BadRequest(user.Message);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("All/Users")]
        public async Task<IActionResult> GetAllAsync([FromQuery] Paging paging)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userService.GetAll(userEmail,paging);
            return user.Status ? Ok(user.Data) : BadRequest(user.Message);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("UserEmail")]
        public async Task<IActionResult> GetUserEmail([FromQuery] string email)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userService.Get(email,userEmail);
            return user.Status ? Ok(user) : BadRequest(user.Message);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] string id)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userService.Delete(id,userEmail);
            return user.Status ? Ok(user) : BadRequest(user.Message);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LogIn( LoginRequestModel model)
        {
            var user = await _userService.Login(model);
            if (user != null)
            {
                var token = _jwtAuthenticationManager.GenerateToken(user.Data);
                var response = new LoginResponseModel(user.Data.Id, token, user.Data.Email,user.Data.FirstName,user.Data.LastName, user.Data.RoleName, user.Data.PhoneNumber,user.Data.UserRoles);
               
                return Ok(response);
            }

            return BadRequest(user);
        }  


    }
}
