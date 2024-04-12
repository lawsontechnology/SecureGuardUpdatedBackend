using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Visitor_Management_System.Core.Application.DTOs;
using Visitor_Management_System.Core.Application.Interface.Services;
using Visitor_Management_System.Infrastructure;

namespace Visitor_Management_System.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

/*        [Authorize(Roles = "Admin")]
*/        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync([FromForm] RoleRequestModel request)
        {
            /*            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;       
            */
            var userEmail = "lawalola04@gmail.com";
            var isSuccessful = await _roleService.Register(request,userEmail);
            if (!isSuccessful.Status)
            {
                return BadRequest(isSuccessful.Message);
            }
            return Ok(isSuccessful);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("All")]
        public async Task<IActionResult> GetAll([FromQuery] Paging paging)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var roles = await _roleService.GetAll(userEmail,paging);
            if (roles == null)
            {
                return BadRequest(roles);
            }
            return Ok(roles.Data);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRole([FromRoute] string id)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var role = await _roleService.Get(id,userEmail);
            if (role == null)
            {
                return BadRequest(role.Message);
            }
            return Ok(role.Data);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Names")]
        public async Task<IActionResult> GetRoles([FromForm] string roleName)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var role = await _roleService.GetUser(roleName,userEmail);
            if (role == null)
            {
                return BadRequest(role.Message);
            }
            return Ok(role);
        }
    }

}
