namespace Visitor_Management_System.Core.Domain
{
    public abstract  class AuditableEntities
    {
        public string Id = Guid.NewGuid().ToString();
        public bool IsDeleted { get; set; }
        public string? DeletedBy { get; set; } 
        public DateTime DateCreated { get; set; }
        
    }
}
