using Visitor_Management_System.Core.Domain.Entities;

namespace Visitor_Management_System.Core.Application.DTOs
{
    public class UserDto
    {
        public string Id { get; private set; } = default!;
        public string Email { get; set; } = default!;
        public string PassWord { get; set; } = default!;
        public Profile Profile { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string? RoleName { get; set; }
        public DateTime DateCreated { get; set; }
        public ICollection<RoleDto> UserRoles { get; set; } = new HashSet<RoleDto>();
        public ICollection<VisitorDto> Visits { get; set; } = new HashSet<VisitorDto>();

        public UserDto(string id)
        {
            Id = id;
        }
    }

    public class LoginRequestModel
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }

    public class LoginResponseModel : UserDto
    {
        public string Token { get; private set; }
        public LoginResponseModel(string id, string token, string email, string firstName, string lastName, string roleName, string phoneNumber, ICollection<RoleDto> userRole) : base(id)
        {
            Token = token;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            RoleName = roleName;
            UserRoles = userRole;

        }
    }

}
