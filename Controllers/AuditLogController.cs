using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Visitor_Management_System.Core.Application.Interface.Services;
using Visitor_Management_System.Core.Application.DTOs;
using System.Security.Claims;
using Visitor_Management_System.Infrastructure;

namespace Visitor_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditLogController : ControllerBase
    {
        private readonly IAuditLogService _auditLogService;

        public AuditLogController(IAuditLogService auditLog)
        {
            _auditLogService = auditLog;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserAsync([FromRoute] string id)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var log = await _auditLogService.Get(id, userEmail);
            if (log == null)
            {
                return NotFound();
            }
            return Ok(log);
        }

        /*[Authorize(Roles = "Admin")]*/
        [HttpGet("All")]
        public async Task<IActionResult> GetAllAsync([FromQuery] Paging paging)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;    
            var log = await _auditLogService.GetAll(userEmail,paging);
            if (log == null)
            {
                return BadRequest(log.Message);
            }
            return Ok(log);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var log = await _auditLogService.Delete(id,userEmail);
            if (!log.Status)
            {
                return BadRequest(log);
            }
            return NoContent(); 
        }



    }

}
