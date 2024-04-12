using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using Visitor_Management_System.Core.Application.Authentication;
using Visitor_Management_System.Core.Application.DTOs;
using Visitor_Management_System.Core.Application.Interface.Services;
using Visitor_Management_System.Infrastructure;

namespace Visitor_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VisitorController : Controller
    {
        private readonly IVisitorService _visitorService;
        public VisitorController(IVisitorService visitorService)
        {
            _visitorService = visitorService;
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var userRole = User.FindFirst("RoleName")?.Value;
            var visitor = await _visitorService.Delete(id,userEmail,userRole);
            if (visitor.Status == false)
            {
                return BadRequest(visitor.Message);
            }
            return Ok(visitor);
        }

        [Authorize(Roles = "Admin,Host")]
        [HttpGet("All")]
        public async Task<IActionResult> GetAllAsync([FromQuery] Paging paging)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var userRole = User.FindFirst("RoleName")?.Value;
            var visitor = await _visitorService.GetAll(userEmail,userRole,paging);
            if (visitor == null)
            {
                return BadRequest(visitor.Message);

            }
            return Ok(visitor.Data);
        }

        [Authorize(Roles = "Admin,Host")]
        [HttpGet("Email")]
        public async Task<IActionResult> GetByEmail([FromQuery] string email)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var userRole = User.FindFirst("RoleName")?.Value;
            var visitor = await _visitorService.GetByEmail(email,userEmail,userRole);
            if (visitor == null) return NotFound();
            return Ok(visitor.Data);
        }

        [Authorize(Roles = "Host")]
        [HttpGet("HostEmail")]
        public async Task<IActionResult> GetByHostEmail([FromQuery] string hostEmail, [FromQuery] Paging paging)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var userRole = User.FindFirst("RoleName")?.Value;
            var visitor = await _visitorService.GetByHostEmail(hostEmail,userEmail,userRole,paging);
            if (visitor == null) return NotFound();
            return Ok(visitor.Data);
        }

        [Authorize(Roles = "Host")]
        [HttpGet("AllApproved/Visit")]
        public async Task<IActionResult> GetVisitByHostEmail([FromQuery] string hostEmail,[FromQuery]Paging paging)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var userRole = User.FindFirst("RoleName")?.Value;
            /*var userRole = User.FindFirst(ClaimTypes.Role)?.Value;*/
            var visitor = await _visitorService.GetVisitByHostEmail(hostEmail,userEmail,userRole,paging);
            if (visitor == null) return NotFound();
            return Ok(visitor.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            var visitor = await _visitorService.GetById(id);
            if (visitor == null) return NotFound();
            return Ok(visitor.Data);
        }

        
        [HttpGet("Visit/Id")]
        public async Task<IActionResult> GetVisit(string visitId)
        {
            var visit = await _visitorService.GetVisit(visitId);
            if (visit == null) return NotFound();
            return Ok(visit.Data);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync(
          [FromForm] VisitorRequestModel model,
          [FromForm] VisitRequestModel visitModel)
        {
            var visitor = await _visitorService.Register(model, visitModel);
            if (visitor.Status == false) return BadRequest(visitor.Message);
            return Ok(visitor.Data);
        }


        [HttpPost("Request/Approve")]
        public async Task<IActionResult> ApproveRequest([FromQuery] string visitId)
        {
            var response = await _visitorService.ApproveRequest(visitId);

            if (response.Status)
            {
                return Ok(response);
            }

            return BadRequest(response.Message);
        }




        [HttpPost("Request/Reject")]
        public async Task<IActionResult> RejectRequest([FromQuery] string visitId)
        {
            var response = await _visitorService.DeniedRequest(visitId);

            if (response.Status)
            {
                return Ok(response);
            }

            return BadRequest(response.Message);
        }

    }

}
