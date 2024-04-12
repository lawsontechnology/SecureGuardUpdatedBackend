namespace Visitor_Management_System.Core.Domain.Entities
{
    public class Address : AuditableEntities
    {
        public string Number { get; set; } = default!;
        public string Street { get; set; } = default!;
        public string City { get; set; } = default!;
        public string State { get; set; } = default!;
        public string? PostalCode { get; set; }
        public Profile Profile { get; set; } = default!;
        public DateTime? DateUpdated { get; set; }
    }
}
