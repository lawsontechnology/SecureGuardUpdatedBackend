using System.ComponentModel.DataAnnotations;
using Visitor_Management_System.Core.Domain.Entities;
using Visitor_Management_System.Core.Domain.Enum;


namespace Visitor_Management_System.Core.Application.DTOs
{
    public class VisitorDto
    {
        public string Id { get;  set; } = default!;
        public string EmailAddress { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string Gender { get; set; }
        public string HostEmail { get; set; } = default!;
        public string Image { get; set; } = default!;
        public DateTime VisitDate { get; set; } = default!;
        public DateTime VisitTime { get; set; } = default!;

        public ICollection<VisitDto> Visits { get; set; } = new HashSet<VisitDto>();

        
    }
    public class ApprovalRequestModel
    {
        public string VisitId { get; set; }
    }
    public class RejectRequestModel
    {
        public string VisitId { get; set; }
    }
    public class VisitorRequestModel
    {
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public Gender Gender { get; set; }
        [DataType(DataType.EmailAddress)]
        public string HostEmail { get; set; } = default!;
        public IFormFile Image { get; set; } = default!;
        
    }
}
