using DocumentFormat.OpenXml.Office2010.Excel;
using Visitor_Management_System.Core.Application.DTOs;
using Visitor_Management_System.Core.Application.Interface.Repositories;
using Visitor_Management_System.Core.Application.Interface.Services;
using Visitor_Management_System.Core.Domain.Entities;
using Visitor_Management_System.Infrastructure;

namespace Visitor_Management_System.Core.Application.Implementation.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IAuditLogRepo _auditLog;
        public AuditLogService(IAuditLogRepo auditLog)
        {
            _auditLog = auditLog;
        }
        public async Task<BaseResponse<AuditLogDto>> Delete(string Id, string userEmail)
        {
            var Audit = await _auditLog.Get(Id);
            if(Audit == null)
            {
                return new BaseResponse<AuditLogDto>
                {
                    Message = "Log Not Found",
                    Status = false,
                };
            }
            var auditLog = new AuditLog
            {
                UserRole = "Admin",
                Timestamp = DateTime.Now,
                UserEmail = userEmail,
                Action = $"auditLog with this Id is been delete {Id}",
                 DateCreated = DateTime.Now,
            };
            Audit.IsDeleted = true;
            await _auditLog.CreateAsync(auditLog);
            await _auditLog.SaveAsync();
            return new BaseResponse<AuditLogDto>
            {
                Message = "Log Is Successfully Deleted",
                Status = true,
            };
        }

        public async Task<BaseResponse<AuditLogDto>> Get(string Id, string userEmail)
        {
            var Audit = await _auditLog.Get(x => x.Id == Id);
            if(Audit == null)
            {
                return new BaseResponse<AuditLogDto>
                {
                    Message = "Log Is Not Found",
                    Status = false,
                };
            }
            var auditLog = new AuditLog
            {
                UserRole = "Admin",
                Timestamp = DateTime.Now,
                UserEmail = userEmail,
                Action = $"Getting this Id :{Id} details ",
                DateCreated = DateTime.Now,
                 
            };
            await _auditLog.CreateAsync(auditLog);
            await _auditLog.SaveAsync();
            return new BaseResponse<AuditLogDto>
            {
                Status = true,
                Message = "Log Is Successfully Retrieved",
                Data = new AuditLogDto()
                {
                    Id = Audit.Id,
                  UserId = Audit.UserId,
                  Action = Audit.Action,
                  Timestamp = Audit.Timestamp,
                   UserEmail=Audit.UserEmail,
                    UserRole = Audit.UserRole,
                }
            };

        }

        public async Task<BaseResponse<ICollection<AuditLogDto>>> GetAll(string userEmail, Paging paging)
        {
            var (auditLogs, totalCount) = await _auditLog.GetAllPagination(paging);

            var listOfAuditLogs = auditLogs.Select(audit => new AuditLogDto
            {
                Id = audit.Id,
                Action = audit.Action,
                Timestamp = audit.Timestamp,
                UserId = audit.UserId,
                UserEmail = audit.UserEmail,
                UserRole = audit.UserRole,
            }).ToList();
            
            var auditLog = new AuditLog
            {
                UserRole = "Admin",
                Timestamp = DateTime.Now,
                UserEmail = userEmail,
                Action = $"Getting All AuditLog From Server ",
                DateCreated = DateTime.Now,
            };

            await _auditLog.CreateAsync(auditLog);
            await _auditLog.SaveAsync();

            return new BaseResponse<ICollection<AuditLogDto>>
            {
                Status = true,
                Message = "Successful Retrieve All",
                Data = listOfAuditLogs,
                TotalCount = totalCount,
                PageNumber = paging.PageNumber,
                PageSize = paging.PageSize
            };
           
        }

    }
}
