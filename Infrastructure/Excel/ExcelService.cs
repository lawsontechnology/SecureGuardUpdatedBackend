using ClosedXML.Excel;
using System.Text.RegularExpressions;
using Visitor_Management_System.Core.Application.DTOs;
using Visitor_Management_System.Core.Application.Interface.Repositories;
using Visitor_Management_System.Core.Application.Interface.Services;
using Visitor_Management_System.Core.Domain.Entities;

namespace Visitor_Management_System.Infrastructure.Excel
{
    public class ExcelService : IExcelService
    {
        private readonly IUserRepo _user;
        private readonly IRoleRepo _role;
        private readonly IProfileRepo _profile;
        private readonly IMailServices _email;
        private readonly IAuditLogRepo _auditLog;
        
        public ExcelService(IUserRepo user, IProfileRepo profile, IRoleRepo role, IMailServices email,IAuditLogRepo auditLog) 
        {
            _user = user;
            _profile = profile;
            _role = role;
            _email = email;
            _auditLog = auditLog;
            
        }

        public async Task<BaseResponse<IEnumerable<UserDto>>> ImportAndSave(ExcelRequestModel model,string userEmail)
        {
            try
            {
                using (var stream = model.File.OpenReadStream())
                {
                    var workbook = new XLWorkbook(stream);
                    var worksheet = workbook.Worksheets.First();
                    var totalRows = worksheet.RowCount();

                    var listOfUserEntities = new List<User>();

                    for (var rowNum = 2; rowNum <= totalRows; rowNum++)
                    {
                        
                        var userEntity = new User
                        {
                            
                            Email = worksheet.Cell(rowNum, 2).Value.ToString(),
                            PassWord = BCrypt.Net.BCrypt.HashPassword("12345"),
                            DateCreated = DateTime.Now,
                        };

                        if (_user.Check(u => u.Email == userEntity.Email))
                        {
                            /*continue;*/
                            return new BaseResponse<IEnumerable<UserDto>>
                            {
                                Message = $"{userEntity.Email} in RowNumber {rowNum} Already Exist !!",
                                Status = false,
                                
                            };
                        }

                        


                        var roleId = model.RoleId;
                        var role = await _role.Get(x => x.Id == roleId && x.IsDeleted == false);
                        if (role == null)
                        {
                            return new BaseResponse<IEnumerable<UserDto>>
                            {
                                Status = false,
                                Message = $"Error occurred while saving the data. Role '{roleId}' does not exist."
                            };
                        }

                        
                        var userRole = new UserRole
                        {
                            Role = role,
                            RoleId = role.Id,
                            DateCreated = DateTime.Now,
                        };

                        userEntity.UserRoles = new List<UserRole> { userRole };

                        var profileEntity = new Profile
                        {
                            
                            FirstName = worksheet.Cell(rowNum, 3).Value.ToString(),
                            LastName = worksheet.Cell(rowNum, 4).Value.ToString(),
                            PhoneNumber = worksheet.Cell(rowNum, 5).Value.ToString(),
                            UserId = userEntity.Id,

                            DateCreated = DateTime.Now,
                        };

                        
                         
                        if (string.IsNullOrEmpty(userEntity.Email) && string.IsNullOrEmpty(profileEntity.FirstName))
                        {
                            break;
                        }

                        if (!IsValidEmail(userEntity.Email) || !IsValidEmails(userEntity.Email))
                        {

                            return new BaseResponse<IEnumerable<UserDto>>
                            {
                                Status = false,
                                Message = $"Invalid email at RowNumber {rowNum}: {userEntity.Email}.",
                            };
                        }

                        if (!IsValidPhoneNumber(profileEntity.PhoneNumber))
                        {
                            return new BaseResponse<IEnumerable<UserDto>>
                            {
                                Status = false,
                                Message = $"Invalid phone number at RowNumber {rowNum} : {profileEntity.PhoneNumber}.",
                            };
                        }

                        var auditLog = new AuditLog
                        {
                            UserRole = "Admin",
                            Timestamp = DateTime.UtcNow,
                            UserEmail = userEmail,
                            Action = $"Sign Up {userEntity.Email} with the use Of Excel ",
                            DateCreated = DateTime.Now,
                             UserId = userEntity.Id,
                        };
                        await _auditLog.CreateAsync(auditLog);

                        userEntity.Profile = profileEntity;
                        
                        await _user.CreateAsync(userEntity);
                        await _profile.CreateAsync(profileEntity);

                        SendRegisterRequestEmail(userEntity, profileEntity);

                    }

                    
                    await _auditLog.SaveAsync();
                    /*await _user.SaveAsync();
                    await _profile.SaveAsync();*/

                    var listOfUserDto = listOfUserEntities.Select(MapToUserDto);

                    return new BaseResponse<IEnumerable<UserDto>>
                    {
                        Status = true,
                        Message = "Data imported and saved successfully.",
                        Data = listOfUserDto
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<UserDto>>
                {
                    Status = false,
                    Message = $"Error importing and saving data: {ex.Message}",
                    Data = null
                };
            }
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
        private bool IsValidPhoneNumber(string phoneNumber)
        {
            const string phoneRegex = @"^(?:(\+234)|0)?([7-9]{1})([0-9]{9})$";
            return Regex.IsMatch(phoneNumber, phoneRegex);
        }


        // Email 
        private void SendRegisterRequestEmail(User user, Profile profile)
        {
            string receiverEmail = user.Email;
            string receiverName = $"{profile.FirstName} {profile.LastName}";
            string subject = "SecureGuard Notification";
            string message = $"<html><body><h2>Pls Kindly Login Using This Link</h2></body></html>\n" +
                             $"<html><body><h3>Update Your Details After Login Is Successful</h3></body></html> :\n" +
                             $"<html><body><h5>FirstName: {profile.FirstName}</h5></body></html>\n" +
                             $"<html><body><h5>LastName: {profile.LastName}</h5></body></html>\n" +
                             $"<html><body><h5>Email: {user.Email}</h5></body></html>\n" +
                             $"<html><body><h5>PhoneNumber: {profile.PhoneNumber}</h5></body></html>\n" +
                             $"<html><body><h5>Password:12345</h5></body></html>\n" +
                             $"<html><body><h4>Login Link: http://127.0.0.1:5505/login.html</h4></body></html>";

            
            var mailRequest = new EMailDto
            {
                ToEmail = receiverEmail,
                ToName = receiverName,
                Subject = subject,
                HtmlContent = message,
                
            };

            _email.SendEMail(mailRequest);
        }


        private UserDto MapToUserDto(User userEntity)
        {
            return new UserDto(userEntity.Id)
            {
                Email = userEntity.Email,
                PassWord = userEntity.PassWord,
                FirstName = userEntity.Profile.FirstName,
                LastName = userEntity.Profile.LastName,
                PhoneNumber = userEntity.Profile.PhoneNumber,
                RoleName = userEntity.UserRoles?.FirstOrDefault()?.Role?.Name,
                Profile = userEntity.Profile,
                 
            };
        }




    }
}
