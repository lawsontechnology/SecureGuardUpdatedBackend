using System.ComponentModel.DataAnnotations;
using Visitor_Management_System.Core.Domain.Entities;
using Visitor_Management_System.Core.Domain.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Visitor_Management_System.Core.Application.DTOs
{
    public class ProfileDto
    {
        public string Id { get; private set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string? PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string? SecurityCode { get; set; }
        public string? Number { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? UserId { get; set; }

        /*public ProfileDto(string id)
        {
            Id = id;
        }*/
    }

    public class RegisterRequestModel
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = default!;
        public string? SecurityCode { get; set; }
        public string Password { get; set; } = default!;
       

    }

      public class ExcelRequestModel
      {
        public string RoleId { get; set; } = default!;
        public IFormFile File { get; set; } = default!;
      }
     
    public class UpdateProfileRequestModel
    {
        public string OldPassword {get; set;} = default!;
        public string NewPassword { get; set; } = default!;
        public string ConfirmPassword { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public Gender Gender { get; set; }
        public string? SecurityCode { get; set; }
        public string Number { get; set; } = default!;
        public string Street { get; set; } = default!;
        public string City { get; set; } = default!;
        public string State { get; set; } = default!;
        public string? PostalCode { get; set; }
    }

    public class AdminProfileRequestModel
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public Gender Gender { get; set; }
        public string Number { get; set; } = default!;
        public string Street { get; set; } = default!;
        public string City { get; set; } = default!;
        public string State { get; set; } = default!;
        public string? PostalCode { get; set; }
    }
}
