namespace Visitor_Management_System.Core.Application.DTOs
{
    public class AuditLogDto
    {
        public string Id { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public string Action { get; set; } = default!;
        public DateTime Timestamp { get; set; }
        public string? UserRole { get; set; } 
        public string? UserEmail { get; set; }

        /*public AuditLogDto(string id)
        {
            Id = id; 
        }*/

    }

    /*public class AuditLogRequestModel
    {
        public string UserId { get; set; } = default!;
        public string? Action { get; set; }
        public DateTime Timestamp { get; set; }

    }*/
}
