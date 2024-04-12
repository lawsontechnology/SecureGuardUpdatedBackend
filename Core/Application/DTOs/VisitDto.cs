using Visitor_Management_System.Core.Domain.Entities;
using Visitor_Management_System.Core.Domain.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Visitor_Management_System.Core.Application.DTOs
{
    public class VisitDto
    {
        public string? Id { get; private set; }
        public User User { get; set; } = default!;
        public Visitor Visitor { get; set; } = default!;
        public DateTime VisitDate { get; set; } = default!;
        public DateTime VisitTime { get; set; } = default!;
        public string VisitReason { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public string VisitorId { get; set; } = default!;
        public string? AddressId { get; set; }
        public string VisitStatus { get; set; } = default!;
        public ICollection<VisitorDto> Visitors { get; set; } = new HashSet<VisitorDto>();
        public ICollection<UserDto> Users { get; set; } = new HashSet<UserDto>();

        public string EmailAddress { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string Gender { get; set; }
        public string HostEmail { get; set; } = default!;
        public string Image { get; set; } = default!;



        /*public VisitDto(string id)
        {
            Id = id;
        }*/
    }

    public class VisitRequestModel
    {
        public DateTime VisitDateAndTime { get; set; } = default!;
        public VisitType VisitReason { get; set; } = default!;
    }

    public class VisitResponseModel
    {
        public string HostEmail { get; set; } = default!;
        public string VisitId { get; set; } = default!;
        public VisitStatus Status { get; set; } = default!;
    }
}
