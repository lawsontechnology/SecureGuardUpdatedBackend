using DocumentFormat.OpenXml.Office2010.Excel;
using Visitor_Management_System.Core.Application.DTOs;
using Visitor_Management_System.Core.Application.Interface.Repositories;
using Visitor_Management_System.Core.Application.Interface.Services;
using Visitor_Management_System.Core.Domain.Entities;
using Visitor_Management_System.Infrastructure;

namespace Visitor_Management_System.Core.Application.Implementation.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepo _role;
        private readonly IAuditLogRepo _auditLog;
        public RoleService(IRoleRepo role,IAuditLogRepo auditLog)
        {
            _role = role;
            _auditLog = auditLog;
        }
        public async Task<BaseResponse<RoleDto>> Get(string id, string userEmail)
        {
            var role = await _role.Get(id);
            if (role == null)
            {
                return new BaseResponse<RoleDto>
                {
                    Message = "Role Not Found",
                    Status = false,
                };
            }
            var auditLog = new AuditLog
            {
                UserRole = "Admin",
                Timestamp = DateTime.UtcNow,
                UserEmail = userEmail,
                Action = $"Getting Role with this Id :{id}, details ",
                DateCreated = DateTime.Now,

            };
            await _auditLog.CreateAsync(auditLog);
            await _auditLog.SaveAsync();
            return new BaseResponse<RoleDto>
            {
                Message = "Role Successfully Retrieved",
                Status = true,
                Data = new RoleDto(role.Id)
                { 
                    Name = role.Name,
                    Description = role.Description,
                    DateCreated = role.DateCreated,
                },
            };
        }

        public async Task<BaseResponse<ICollection<RoleDto>>> GetAll(string userEmail, Paging paging)
        {
            List<RoleDto> listOfRole = new List<RoleDto>();
            var rol = await _role.GetAll(paging);
            foreach (var roles in rol)
            {
                var role = new RoleDto(roles.Id)
                {

                    Name = roles.Name,
                    Description = roles.Description,
                    DateCreated = roles.DateCreated,
                };
                listOfRole.Add(role);
            }
            var auditLog = new AuditLog
            {
                UserRole = "Admin",
                Timestamp = DateTime.Now,
                UserEmail = userEmail,
                Action = $"Getting All Roles ",
                DateCreated = DateTime.Now,

            };
            await _auditLog.CreateAsync(auditLog);
            await _auditLog.SaveAsync();
            return new BaseResponse<ICollection<RoleDto>>
            {
                Status = true,
                Message = "List of Role",
                Data = listOfRole,
            };
        }

        public async Task<BaseResponse<RoleDto>> GetUser(string RoleName, string userEmail)
        {
            var role = await _role.Get(x => x.Name == RoleName && x.IsDeleted == false);
            if (role == null)
            {
                return new BaseResponse<RoleDto>
                {
                    Message = "Role Not Found",
                    Status = false,
                };
            }
            var auditLog = new AuditLog
            {
                UserRole = "Admin",
                Timestamp = DateTime.Now,
                UserEmail = userEmail,
                Action = $"Getting this RoleName :{RoleName}, details ",
                DateCreated = DateTime.Now,
                 
            };
            await _auditLog.CreateAsync(auditLog);
            await _auditLog.SaveAsync();
            return new BaseResponse<RoleDto>
            {
                Message = "Role Successfully Retrieved",
                Status = true,
                Data = new RoleDto(role.Id)
                {
                    Name = role.Name,
                    Description = role.Description,
                    DateCreated = role.DateCreated,
                },
            };
        }

        public async Task<BaseResponse<RoleDto>> Register(RoleRequestModel model,string userEmail)
        {
            var role = await _role.Get(x => x.Name == model.Name && x.IsDeleted == false);
            if (role != null)
            {
                return new BaseResponse<RoleDto>
                {
                    Message = "Role Already Exist",
                    Status = false
                };
            }
            var newRole = new Role
            {
                Name = model.Name,
                Description = model.Description,
                DateCreated = DateTime.Now,
            };

            var isSuccessful = await _role.CreateAsync(newRole);
            if (isSuccessful == null)
            {
                return new BaseResponse<RoleDto>
                {
                    Message = "Unable to Add Role",
                    Status = false
                };
            }
            var auditLog = new AuditLog
            {
                UserRole = "Admin",
                Timestamp = DateTime.Now,
                UserEmail = userEmail,
                Action = $"Creating {model.Name} Role",
                DateCreated = DateTime.Now,

            };
            await _auditLog.CreateAsync(auditLog);
            await _auditLog.SaveAsync();
            return new BaseResponse<RoleDto>
            {
                Message = "Role Successfully Created",
                Status = true,
                Data = new RoleDto(newRole.Id)
                {
                    /*Id = newRole.Id,*/
                    Name = newRole.Name,
                    Description = newRole.Description,
                    DateCreated = newRole.DateCreated,

                }
            };
        }
    }
}
