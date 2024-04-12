using Visitor_Management_System.Core.Domain.Enum;

namespace Visitor_Management_System.Core.Domain.Entities
{
    public class Visitor : AuditableEntities
    {
        public string EmailAddress { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public Gender Gender { get; set; }
        public string HostEmail { get; set; } = default!;
        public string Image { get; set; } = default!;
        public ICollection<Visit> Visits { get; set; } = new HashSet<Visit>();

    }
}
