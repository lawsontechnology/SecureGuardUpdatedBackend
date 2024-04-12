namespace Visitor_Management_System.Core.Domain.Entities
{
    public class User : AuditableEntities
    {
        public string Email { get; set; } = default!;
        public string PassWord { get; set; } = default!;
        public Profile Profile { get; set; } = default!;
        public ICollection<UserRole> UserRoles { get; set; } =new HashSet<UserRole>();
        public ICollection<Visit> Visits { get; set; } = new HashSet<Visit>();
    }
}
