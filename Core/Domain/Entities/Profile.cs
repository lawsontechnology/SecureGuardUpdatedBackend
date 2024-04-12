using System.Reflection;
using Visitor_Management_System.Core.Domain.Enum;

namespace Visitor_Management_System.Core.Domain.Entities
{
    public class Profile : AuditableEntities
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;    
        public string? PhoneNumber { get; set; } 
        public Gender Gender { get; set; }
        public string? AddressId { get; set; } 
        public Address Address { get; set; } 
        public User User { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public DateTime? DateUpdated { get; set; }
    }
}
