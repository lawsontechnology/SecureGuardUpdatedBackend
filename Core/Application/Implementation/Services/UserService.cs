using System.Text.RegularExpressions;
using Visitor_Management_System.Core.Application.DTOs;
using Visitor_Management_System.Core.Application.Interface.Repositories;
using Visitor_Management_System.Core.Application.Interface.Services;
using Visitor_Management_System.Core.Domain.Entities;
using Visitor_Management_System.Core.Domain.Enum;
using Visitor_Management_System.Infrastructure;

namespace Visitor_Management_System.Core.Application.Implementation.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _user;
        private readonly IProfileRepo _profile;
        private readonly IAddressRepo _address;
        private readonly IAuditLogRepo _auditLog;
        private readonly IRoleRepo _role;
        private readonly IMailServices _email;

        public UserService(IProfileRepo profile,IAuditLogRepo auditLog, IAddressRepo address,IUserRepo user,IRoleRepo role, IMailServices email)
        {
            _profile = profile;
            _address = address;
            _email = email;
            _user = user;
            _auditLog = auditLog;
            _role = role;
           
        }
        public async Task<BaseResponse<UserDto>> Delete(string Id, string userEmail)
        {
            var users = await _user.Get(Id);
            if (users == null)
            {
                return new BaseResponse<UserDto>

                {
                    Message = "User Not Found",
                    Status = false,
                };
            }
            var auditLog = new AuditLog
            {
                  UserId = users.Id,
                  Action = $"delete User {users.Profile.FirstName +""+users.Profile.LastName} from the application",
                  Timestamp = DateTime.Now,
                  UserRole ="Admin",
                  DateCreated = DateTime.Now,
                  UserEmail = userEmail,
                      

            };
            users.Profile.IsDeleted = true;
            users.IsDeleted = true;
            await _auditLog.CreateAsync(auditLog);
            await _auditLog.SaveAsync();
            
            return new BaseResponse<UserDto>
            {
                Message = "User Successfully Deleted",
                Status = true,
            };
        }

        public async Task<BaseResponse<UserDto>> Get(string email, string userEmail)
        {
            var users = await _user.Get(x => x.Email == email);
            if (users == null)
            {
                return new BaseResponse<UserDto>
                {
                    Message = "Not Found",
                    Status = false,
                };
            }
            var auditLog = new AuditLog
            {
                UserId = users.Id,
                Action = $"Retrieve this User {email} Details",
                Timestamp = DateTime.UtcNow,
                UserRole = "Admin",
                UserEmail = userEmail,
                DateCreated = DateTime.Now,
                 

            };
            await _auditLog.CreateAsync(auditLog);
            await _auditLog.SaveAsync();
            return new BaseResponse<UserDto>
            {
                Status = true,
                Message = "Successfully Retrieved",
                Data = new UserDto(users.Id)
                {
                    Email = users.Email,
                    PassWord = users.PassWord,
                    FirstName = users.Profile.FirstName,
                    LastName = users.Profile.LastName,
                    PhoneNumber = users.Profile.PhoneNumber,
                    
                    UserRoles = users.UserRoles.Select(x => new RoleDto(x.RoleId)
                    {
                        Name = x.Role.Name,
                        Description = x.Role.Description,

                    }).ToList(),


                }
            };
        }

        public async Task<BaseResponse<ICollection<UserDto>>> GetAll(string userEmail, Paging paging)
        {
            List<UserDto> listOfUsers = new List<UserDto>();
            var users = await _user.GetAll(paging);
            foreach (var user in users)
            {
                var profile = await _user.Get(user.Profile.Id);
                var userList = new UserDto(user.Id)
                {
                    Email = user.Email,
                    FirstName = user.Profile.FirstName,
                    LastName = user.Profile.LastName,
                    PhoneNumber = user.Profile.PhoneNumber,
                    RoleName = user.UserRoles.FirstOrDefault()?.Role?.Name,
                    UserRoles = user.UserRoles.Select(x => new RoleDto(x.Role.Id)
                    {
                        Name = x.Role.Name,
                        Description = x.Role.Description,

                    }).ToList(),

                };
                listOfUsers.Add(userList);
            }
            var auditLog = new AuditLog
            {
                Action = $"Retrieving Of All User Details",
                Timestamp = DateTime.UtcNow,
                DateCreated = DateTime.Now,
                UserEmail = userEmail,
                UserRole = "Admin",
                    
            };
            await _auditLog.CreateAsync(auditLog);
            await _auditLog.SaveAsync();
            return new BaseResponse<ICollection<UserDto>>
            {
                Status = true,
                Message = "Successful",
                Data = listOfUsers,
            };
        }

        public async Task<BaseResponse<ICollection<UserDto>>> GetAllHost(string userEmail, Paging paging)
        {
            List<UserDto> listOfHosts = new List<UserDto>();
            var users = await _user.GetAll(paging);

            foreach (var user in users)
            {
                var profile = await _user.Get(user.Id);

                if (user.UserRoles.Any(role => role.Role.Name == "Host"))
                {
                    var hostDto = new UserDto(user.Id)
                    {
                        Email = user.Email,
                        FirstName = user.Profile.FirstName,
                        LastName = user.Profile.LastName,
                        PhoneNumber = user.Profile.PhoneNumber,
                        UserRoles = user.UserRoles.Select(x => new RoleDto(x.Role.Id)
                        {
                            Name = x.Role.Name,
                            Description = x.Role.Description,

                        }).ToList(),
                    };

                    listOfHosts.Add(hostDto);
                }
            }
            var auditLog = new AuditLog
            {
                Action = $" Retrieved Of All Host Details",
                Timestamp = DateTime.UtcNow,
                UserRole = "Admin",
                UserEmail = userEmail,
                DateCreated = DateTime.Now,
            };
            await _auditLog.CreateAsync(auditLog);
            await _auditLog.SaveAsync();
            return new BaseResponse<ICollection<UserDto>>
            {
                Status = true,
                Message = "Successful",
                Data = listOfHosts,
            };
        }


        public async Task<BaseResponse<ICollection<UserDto>>> GetAllSecurity(string userEmail, Paging paging)
        {
            List<UserDto> listOfSecurities = new List<UserDto>();
            var users = await _user.GetAll(paging);

            foreach (var user in users)
            {
                var profile = await _user.Get(user.Id);

                if (user.UserRoles.Any(role => role.Role.Name == "Security"))
                {
                    var securityDto = new UserDto(user.Id)
                    {
                        Email = user.Email,
                        FirstName = user.Profile.FirstName,
                        LastName = user.Profile.LastName,
                        PhoneNumber = user.Profile.PhoneNumber,
                        UserRoles = user.UserRoles.Select(x => new RoleDto(x.Role.Id)
                        {
                            Name = x.Role.Name,
                            Description = x.Role.Description,

                        }).ToList(),
                    };

                    listOfSecurities.Add(securityDto);
                }
            }
            var auditLog = new AuditLog
            {
                Action = $"Retrieved of All Security Details",
                Timestamp = DateTime.UtcNow,
                DateCreated = DateTime.Now,
                UserEmail = userEmail,
                UserRole = "Admin",
            };
            await _auditLog.CreateAsync(auditLog);
            await _auditLog.SaveAsync();
            return new BaseResponse<ICollection<UserDto>>
            {
                Status = true,
                Message = "Successful",
                Data = listOfSecurities,
            };
        }

        public async Task<BaseResponse<UserDto>> GetById(string id)
        {
            var users = await _user.Get(id);
            if (users == null)
            {
                return new BaseResponse<UserDto>
                {
                    Message = "Not Found",
                    Status = false,
                };
            }
            var auditLog = new AuditLog
            {
                UserId = users.Id,
                Action = $"Retrieve of {users.Email} Info",
                Timestamp = DateTime.UtcNow,
                UserRole = "Admin",
                DateCreated = DateTime.Now,
                 
            };
            await _auditLog.CreateAsync(auditLog);
            await _auditLog.SaveAsync();
            return new BaseResponse<UserDto>
            {
                Status = true,
                Message = "Successfully Retrieved",
                Data = new UserDto(users.Id)
                {
                    Email = users.Email,
                    PassWord = users.PassWord,
                    FirstName = users.Profile.FirstName,
                    LastName = users.Profile.LastName,
                    DateCreated = users.DateCreated,
                    PhoneNumber = users.Profile.PhoneNumber,
                     
                    UserRoles = users.UserRoles.Select(x => new RoleDto(x.Role.Id)
                    {
                        Name = x.Role.Name,
                        Description = x.Role.Description,

                    }).ToList(),


                }
            };
        }

       



        public async Task<BaseResponse<UserDto>> Login(LoginRequestModel model)
        {
            var user = await _user.Get(x => x.Email == model.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PassWord))
            {
                return new BaseResponse<UserDto>
                {
                    Message = "Invalid Email or password",
                    Status = false,
                };
            }

            var auditLog = new AuditLog
            {
                UserId = user.Id,
                Action = $"{user.Email} Login Into The Application ",
                Timestamp = DateTime.UtcNow,
                DateCreated = DateTime.Now,
                UserEmail = user.Email,
                 
            };
            await _auditLog.CreateAsync(auditLog);
            await _auditLog.SaveAsync();
            return new BaseResponse<UserDto>
            {
                Message = "User successfully Logged In",
                Status = true,
                Data = new UserDto(user.Id)
                {
                    PhoneNumber = user.Profile.PhoneNumber,
                    Email = user.Email,
                    FirstName = user.Profile.FirstName,
                    LastName = user.Profile.LastName,
                    RoleName = user.UserRoles.FirstOrDefault()?.Role?.Name,
                    UserRoles = user.UserRoles.Select(role => new RoleDto(role.Role.Id)
                    {
                        Name = role.Role.Name,
                        Description = role.Role.Description,
                    }).ToList(),

                },

            };
        }

        public async Task<BaseResponse<UserDto>> RegisterAdmin(AdminProfileRequestModel model)
        {
            var host = _user.Check(x => x.Email == model.Email);
            if (host == true)
            {
                return new BaseResponse<UserDto>
                {
                    Message = "User already exist",
                    Status = false,
                };
            }

            if (!IsValidEmail(model.Email))
            {
                return new BaseResponse<UserDto>
                {
                    Status = false,
                    Message = $"Invalid email : {model.Email}.",
                };
            }
            var user = new User()
            {
                Email = model.Email,
                PassWord = BCrypt.Net.BCrypt.HashPassword(model.Password),
                DateCreated = DateTime.Now,
            };



            var address = new Address()
            {
                Street = model.Street,
                State = model.State,
                Number = model.Number,
                City = model.City,
                PostalCode = model.PostalCode,
                DateCreated = DateTime.Now,
            };

            var profile = new Profile()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                User = user,
                UserId = user.Id,
                DateCreated = DateTime.Now,
                Address = address,
                AddressId = address.Id,
                PhoneNumber = model.PhoneNumber,
                Gender = model.Gender,
                     
            };
            var role = await _role.Get(x => x.Name == "Admin");
            if (role == null)
            {
                return new BaseResponse<UserDto>
                {
                    Message = "Unable Find Admin Role",
                    Status = false
                };
            }

            var auditLog = new AuditLog
            {
                UserId = user.Id,
                Action = $" Creating of new Admin Name: {model.FirstName + "" + model.LastName}",
                Timestamp = DateTime.UtcNow,
                 DateCreated= DateTime.Now,
            };

            var userRole = new UserRole
            {
                User = user,
                UserId = user.Id,
                Role = role,
                RoleId = role.Id,
                DateCreated = DateTime.Now,
            };

            await _user.CreateAsync(user);
            user.UserRoles.Add(userRole);
            await _profile.CreateAsync(profile);
            await _address.CreateAsync(address);
            await _auditLog.CreateAsync(auditLog);
            SendRequestEmail(user);
            await _user.SaveAsync();
            await _profile.SaveAsync();
            await _address.SaveAsync();
            await _auditLog.SaveAsync();
            return new BaseResponse<UserDto>
            {
                Message = "Admin Created Successfully",
                Status = true,
                Data = new UserDto(user.Id)
                {
                    Email = user.Email,
                    PassWord = user.PassWord,
                    FirstName = profile.FirstName,
                     PhoneNumber = profile.PhoneNumber,
                    LastName = profile.LastName,
                     RoleName = role.Name,
                    UserRoles = user.UserRoles.Select(x => new RoleDto(x.RoleId)
                    {
                        Name = x.Role.Name,
                        Description = x.Role.Description,

                    }).ToList(),
                }
            };
        }

        public async Task<BaseResponse<UserDto>> UserRegister(RegisterRequestModel model, string userEmail)
        {
            var host = _user.Check(x => x.Email == model.Email);
            if (host == true)
            {
                return new BaseResponse<UserDto>
                {
                    Message = "User already exist",
                    Status = false,
                };
            }

            if (!IsValidEmail(model.Email) || !IsValidEmails(model.Email))
            {
                return new BaseResponse<UserDto>
                {
                    Status = false,
                    Message = $"Invalid email : {model.Email}. Pls input Correct Email",
                };
            }
            var user = new User()
            {
                Email = model.Email,
                PassWord = BCrypt.Net.BCrypt.HashPassword(model.Password),
                DateCreated = DateTime.Now,
            };


            var profile = new Profile()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                User = user,
                UserId = user.Id,
                DateCreated = DateTime.Now,

            };
            var RoleName = "";
            if(model.SecurityCode != null)
            {
                RoleName = "Security";
            }
            if(model.SecurityCode == null)
            {
                RoleName = "Host";
            }
            var role = await _role.Get(x => x.Name == RoleName);
            if (role == null)
            {
                return new BaseResponse<UserDto>
                {
                    Message = $"Unable Find {RoleName} Role",
                    Status = false
                };
            }

            var auditLog = new AuditLog
            {
                UserId = user.Id,
                Action = $" Creating of new {RoleName} Name: {model.FirstName + "" + model.LastName}",
                Timestamp = DateTime.UtcNow,
                UserEmail = userEmail,
                UserRole = "Admin",
                DateCreated = DateTime.Now,
            };

            var userRole = new UserRole
            {
                User = user,
                UserId = user.Id,
                Role = role,
                RoleId = role.Id,
                DateCreated = DateTime.Now,
            };

            await _user.CreateAsync(user);
            user.UserRoles.Add(userRole);
            await _profile.CreateAsync(profile);
            await _auditLog.CreateAsync(auditLog);
            SendRequestEmail(user);
            await _user.SaveAsync();
            await _profile.SaveAsync();
            await _auditLog.SaveAsync();
            return new BaseResponse<UserDto>
            {
                Message = "User Created Successfully",
                Status = true,
                Data = new UserDto(user.Id)
                {
                    Email = user.Email,
                    PassWord = user.PassWord,
                    FirstName = profile.FirstName,
                    PhoneNumber = profile.PhoneNumber,
                    LastName = profile.LastName,
                    RoleName = role.Name,
                    UserRoles = user.UserRoles.Select(x => new RoleDto(x.RoleId)
                    {
                        Name = x.Role.Name,
                        Description = x.Role.Description,

                    }).ToList(),
                }
            };
        }

        public async Task<BaseResponse<UserDto>> Update(string id, UpdateProfileRequestModel model, string userRole)
        {
            var user = await _user.Get(id);

            if (user == null)
            {
                return new BaseResponse<UserDto>
                {
                    Status = false,
                    Message = "User Not Found",
                };
            }

            var address = user.Profile?.Address ?? new Address();
            address.City = model.City;
            address.Number = model.Number;
            address.PostalCode = model.PostalCode;
            address.State = model.State;
            address.Street = model.Street;
            address.DateUpdated = DateTime.Now;

            var profile = user.Profile ?? new Profile();
            
            profile.Address = address;
            profile.PhoneNumber = model.PhoneNumber;
            profile.Gender = (Gender)Enum.ToObject(typeof(Gender), model.Gender);
            profile.DateUpdated = DateTime.Now;

            var auditLog = new AuditLog
            {
                UserId = user.Id,
                Action = $"Updating {user.Email} Profile",
                Timestamp = DateTime.Now,
                UserRole = userRole,
                UserEmail = user.Email,
            };

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.OldPassword, user.PassWord))
            {
                return new BaseResponse<UserDto>
                {
                    Message = "Invalid  password",
                    Status = false,
                };
            }

            if(model.NewPassword != model.ConfirmPassword)
            {
                return new BaseResponse<UserDto>
                {
                    Message = "Password Not Match",
                    Status = false,
                };
            }
           if(model.NewPassword != null)
            {
            user.PassWord = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
            user.DateCreated = DateTime.Now;
                SendUpdateEmail(user, model);
            }

            await _auditLog.CreateAsync(auditLog);
            await _user.SaveAsync();
            await _address.SaveAsync();
            await _profile.SaveAsync();
            await _auditLog.SaveAsync();

            return new BaseResponse<UserDto>
            {
                Message = "User Successfully Updated",
                Status = true,
                Data = new UserDto(user.Id)
                {
                     Email = user.Email,
                     FirstName = user.Profile.FirstName,
                     LastName = user.Profile.LastName,
                     PhoneNumber = user.Profile.PhoneNumber,
                      UserRoles = user.UserRoles.Select(x => new RoleDto(x.RoleId)
                    {
                        Name = x.Role.Name,
                        Description = x.Role.Description,

                    }).ToList(),
                }
            };
        }

        private void SendRequestEmail(User user)
        {
            string receiverEmail = user.Email;
            string receiverName = $"{user.Profile.FirstName} {user.Profile.LastName}";
            string subject = " SecureGuard Guest Notification";
            string message = $"<html><body><h2>Successful SignUp</h2></body></html>\n" +
                             $"<html><body><h3>Your Details</h3></body></html> :\n" +
                             $"<html><body><h4>FullName: {user.Profile.FirstName} {user.Profile.LastName}</h4></body></html>\n" +
                             $"<html><body><h4>Email: {user.Email}</h4></body></html>\n" +
                             $"<html><body><h4>PhoneNumber: {user.Profile.PhoneNumber}</h4></body></html>\n" +
                             $"<html><body><h4>Password: {"12345"}</h4></body></html>\n" +
                             $"<html><body><h4>Login Link: http://127.0.0.1:5505/login.html</h4></body></html>";


            var emailDto = new EMailDto
            {
                ToEmail = receiverEmail,
                ToName = receiverName,
                Subject = subject,
                HtmlContent = message,
            };

            _email.SendEMail(emailDto);
        }
        private bool IsValidEmail(string email)
        {
            const string emailRegex = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, emailRegex);
        }

        private bool IsValidEmails(string email)
        {
            const string emailRegex = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, emailRegex);
        }

        private void SendUpdateEmail(User user,UpdateProfileRequestModel update)
        {
            string receiverEmail = user.Email;
            string receiverName = $"{user.Profile.FirstName} {user.Profile.LastName}";
            string subject = " SecureGuard Guest Notification";
            string message = $"<html><body><h2>Successful Update Password </h2></body></html>\n" +
                             $"<html><body><h3>Details</h3></body></html> :\n" +
                             $"<html><body><h4>Password: {update.NewPassword}</h4></body></html>";


            var emailDto = new EMailDto
            {
                ToEmail = receiverEmail,
                ToName = receiverName,
                Subject = subject,
                HtmlContent = message,
            };

            _email.SendEMail(emailDto);
        }
    }
}
