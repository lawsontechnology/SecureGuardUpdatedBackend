using Visitor_Management_System.Core.Domain.Enum;

namespace Visitor_Management_System.Core.Domain.Entities
{
    public class Visit : AuditableEntities
    {
        public User User { get; set; } = default!;
        public Visitor Visitor { get; set; } = default!;
        public DateTime VisitDate { get; set; } = default!;
        public DateTime VisitTime { get; set; } = default!;
        public VisitType VisitReason { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public string VisitorId { get; set; } = default!;
        public VisitStatus VisitStatus { get; set; } = default!;
    }
}
