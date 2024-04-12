using Visitor_Management_System.Core.Domain.Entities;

namespace Visitor_Management_System.Core.Application.DTOs
{
    public class RoleDto
    {
        public string Id { get; private set; } = default!;
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public DateTime DateCreated { get; set; }
        public ICollection<UserDto> UserRoles { get; set; } = new HashSet<UserDto>();

        public RoleDto(string id)
        {
            Id = id;
        }
    }

    public class RoleRequestModel
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
    }

    public class RoleUpdateModel
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
    }
}
