using Visitor_Management_System.Core.Domain.Entities;

namespace Visitor_Management_System.Core.Application.DTOs
{
    public class AddressDto
    {

        public string Id { get; private set; } = default!;
        public string Number { get; set; } = default!;
        public string Street { get; set; } = default!;
        public string City { get; set; } = default!;
        public string State { get; set; } = default!;
        public string? PostalCode { get; set; }
        public Profile Profile { get; set; } = default!;

        public AddressDto(string id)
        {
            Id = id;
        }
    }
}
