namespace Visitor_Management_System.Core.Domain.Entities
{
    public class Role : AuditableEntities
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>(); 
        
    }
}
