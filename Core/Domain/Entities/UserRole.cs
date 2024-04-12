namespace Visitor_Management_System.Core.Domain.Entities
{
    public class UserRole : AuditableEntities
    {
        public string UserId { get; set; } = default!;
        public string RoleId { get; set; } = default!;
        public Role Role { get; set; } = default!;
        public User User { get; set; } = default!;
    }
}
