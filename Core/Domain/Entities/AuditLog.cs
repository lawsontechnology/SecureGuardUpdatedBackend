using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Visitor_Management_System.Core.Domain.Entities
{

    public class AuditLog : AuditableEntities
    {
        public string? UserId { get; set; }
        public string Action { get; set; } = default!;
        public DateTime Timestamp { get; set; }
        public string? UserEmail { get; set; }
        public string? UserRole { get; set; }

    }
}