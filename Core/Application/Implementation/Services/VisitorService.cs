using System.Net.Mail;
using System.Text.RegularExpressions;
using Visitor_Management_System.Core.Application.Interface.Repositories;
using Visitor_Management_System.Core.Application.Interface.Services;
using Visitor_Management_System.Core.Domain.Entities;
using Visitor_Management_System.Core.Domain.Enum;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Net.Mime;
using Visitor_Management_System.Core.Application.DTOs;
using Visitor_Management_System.Infrastructure.Email;
using Visitor_Management_System.Infrastructure;

namespace Visitor_Management_System.Core.Application.Implementation.Services
{
    public class VisitorService : IVisitorService
    {
        private readonly IVisitRepo _visit;
        private readonly IUserRepo     _user;
        private readonly IVisitorRepo _visitor;
        private readonly IAuditLogRepo _auditLog;
        private readonly IMailServices _email;
        private readonly ITokenService _tokenService;
        public VisitorService( IUserRepo user, IVisitRepo visit,IVisitorRepo visitor,IAuditLogRepo auditLog, ITokenService tokenService, IMailServices email)
        {
            _user = user;
            _visit = visit;
            _tokenService = tokenService;     
            _visitor = visitor;
            _auditLog = auditLog;
            _email = email;

        }
        public async Task<BaseResponse<VisitorDto>> Delete(string Id, string userEmail, string userRole)
        {
            var visitors = await _visitor.Get(Id);
            if (visitors == null)
            {
                return new BaseResponse<VisitorDto>
                {
                    Message = "User Not Found",
                    Status = false,
                };
            }
            var auditLog = new AuditLog
            {
                UserId = visitors.Id,
                Action = $"This Visitor: {visitors.FirstName +""+ visitors.LastName} with this Email {visitors.EmailAddress} Info as been deleted",
                Timestamp = DateTime.UtcNow,
                DateCreated = DateTime.Now,
                UserEmail = userEmail,
                UserRole = userRole,
                
            };
            await _auditLog.CreateAsync(auditLog);
            visitors.IsDeleted = true;
            await _visitor.SaveAsync();
            
            return new BaseResponse<VisitorDto>
            {
                Message = "User Successfully Deleted",
                Status = true,
            };
        }

        public async Task<BaseResponse<ICollection<VisitorDto>>> GetAll(string userEmail, string userRole, Paging paging)
        {
            try
            {
                var visitors = await _visitor.GetAll(paging);

                var listOfVisitors = visitors.Select(visitor => new VisitorDto
                {
                    EmailAddress = visitor.EmailAddress,
                    FirstName = visitor.FirstName,
                    Gender = visitor.Gender.ToString(),
                    HostEmail = visitor.HostEmail,
                    Image = visitor.Image,
                    PhoneNumber = visitor.PhoneNumber,
                    LastName = visitor.LastName,
                    Visits = visitor.Visits.Select(x => new VisitDto
                    {
                        VisitDate = x.VisitDate,
                        VisitorId = x.VisitorId,
                        VisitTime = x.VisitTime,
                        VisitReason = x.VisitReason.ToString(),
                        VisitStatus = x.VisitStatus.ToString(),
                    }).ToList(),
                }).ToList();

                var auditLog = new AuditLog
                {
                    Action = "Getting All Visitors In The Application",
                    Timestamp = DateTime.UtcNow,
                    UserRole = userRole,
                    UserEmail = userEmail,
                    DateCreated = DateTime.Now,
                };

                await _auditLog.CreateAsync(auditLog);
                await _auditLog.SaveAsync();

                return new BaseResponse<ICollection<VisitorDto>>
                {
                    Status = true,
                    Message = "Successful",
                    Data = listOfVisitors,
                };
            }
            catch (Exception ex)
            {
                
                return new BaseResponse<ICollection<VisitorDto>>
                {
                    Status = false,
                    Message = $"Error: {ex.Message}",
                    Data = null,
                };
            }
        }



        public async Task<BaseResponse<VisitorDto>> GetByEmail(string email, string userEmail, string userRole)
        {
            var visit = await _visitor.Get(x => x.EmailAddress == email);
            if (visit == null) 
            {
                return new BaseResponse<VisitorDto>
                {
                    Message = "Visitor Not Found",
                    Status = false,
                };
            }
            var auditLog = new AuditLog
            {
                UserId = visit.Id,
                Action = $"Retrieving Visitor Name: {visit.FirstName +""+visit.LastName}with this userEmail {email}",
                Timestamp = DateTime.UtcNow,
                UserEmail = userEmail,
                UserRole = userRole,
                DateCreated = DateTime.Now,
            };
            await _auditLog.CreateAsync(auditLog);
            await _auditLog.SaveAsync();
            return new BaseResponse<VisitorDto>
            {
                Status = true,
                Message = "Visitor Successfully Retrieved",
                Data = new VisitorDto
                {
                    EmailAddress = visit.EmailAddress,
                    FirstName = visit.FirstName,
                    LastName = visit.LastName,
                    PhoneNumber = visit.PhoneNumber,
                    HostEmail = visit.HostEmail,
                    Image = visit.Image,
                    Gender = visit.Gender.ToString(),
                    Visits = visit.Visits.Select(x => new VisitDto
                    {
                        VisitDate = x.VisitDate,
                        VisitorId = x.VisitorId,
                        VisitTime = x.VisitTime,
                        VisitReason = x.VisitReason.ToString(),
                        VisitStatus = x.VisitStatus.ToString(),
                    }).ToList(),
                }
            };
        }

        public async Task<BaseResponse<ICollection<VisitorDto>>> GetByHostEmail(string hostEmail, string userEmail, string userRole, Paging paging)
        {
            var visit = await _visitor.GetAll(x => x.HostEmail == hostEmail,paging);
            if (visit == null)
            {
                return new BaseResponse<ICollection<VisitorDto>>
                {
                    Message = "Visitor Not Found Or Email Not Exist",
                    Status = false,
                };
            }
            var listOfVisitors = visit.Select(visitor => new VisitorDto
            {
                EmailAddress = visitor.EmailAddress,
                FirstName = visitor.FirstName,
                Gender = visitor.Gender.ToString(),
                HostEmail = visitor.HostEmail,
                Image = visitor.Image,
                PhoneNumber = visitor.PhoneNumber,
                LastName = visitor.LastName,
                Visits = visitor.Visits.Select(x => new VisitDto
                {
                    VisitDate = x.VisitDate,
                    VisitorId = x.VisitorId,
                    VisitTime = x.VisitTime,
                    VisitReason = x.VisitReason.ToString(),
                    VisitStatus = x.VisitStatus.ToString(),
                }).ToList(),
            }).ToList();
            var auditLog = new AuditLog
            {
                Action = $"Retrieving All Visitors of Host With This Email{hostEmail}",
                Timestamp = DateTime.UtcNow,
                UserEmail = userEmail,
                UserRole = userRole,
                DateCreated = DateTime.Now,
                
            };
            await _auditLog.CreateAsync(auditLog);
            await _auditLog.SaveAsync();
            return new BaseResponse<ICollection<VisitorDto>>
            {
                Status = true,
                Message = "Visitor Successfully Retrieved",
                Data = listOfVisitors,
                
            };
        }

        public async Task<BaseResponse<ICollection<VisitDto>>> GetVisitByHostEmail(string hostEmail, string userEmail, string userRole, Paging paging)
        {
            var hostExists =  _visitor.Check(x => x.HostEmail == hostEmail);
            if (!hostExists)
            {
                return new BaseResponse<ICollection<VisitDto>>
                {
                    Message = "Host Email Not Found",
                    Status = false,
                };
            }

            var approvedVisits = await _visit.GetAll(x => x.Visitor.HostEmail == hostEmail && x.VisitStatus == VisitStatus.Approved, paging);

            if (approvedVisits == null)
            {
                return new BaseResponse<ICollection<VisitDto>>
                {
                    Message = "No Approved Visits found for the Host Email",
                    Status = false,
                };
            }
            var listOfVisit = approvedVisits.Select(Approved => new VisitDto
            {
                VisitStatus = Approved.VisitStatus.ToString(),
                VisitReason = Approved.VisitReason.ToString(),
                VisitTime = Approved.VisitTime,
                VisitDate = Approved.VisitTime,
                EmailAddress = Approved.Visitor.EmailAddress,
                FirstName = Approved.Visitor.FirstName,
                LastName = Approved.Visitor.LastName,
                PhoneNumber = Approved.Visitor.PhoneNumber,
                Image = Approved.Visitor.Image
                

            }).ToList();
            var auditLog = new AuditLog
            {
                Action = $"Retrieving Approved Visits for Host Email: {hostEmail}",
                Timestamp = DateTime.Now,
                UserRole = userRole,
                UserEmail = userEmail,
                DateCreated = DateTime.Now,
               
            };
            await _auditLog.CreateAsync(auditLog);
            await _auditLog.SaveAsync();

            return new BaseResponse<ICollection<VisitDto>>
            {
                Status = true,
                Message = "Approved Visits Successfully Retrieved",
                Data = listOfVisit,
                
            };
        }


        public async Task<BaseResponse<VisitorDto>> GetById(string id)
        {
            var visit = await _visitor.Get(id);
            if (visit == null)
            {
                return new BaseResponse<VisitorDto>
                {
                    Message = "Visitor Not Found",
                    Status = false,
                };
            }
            var auditLog = new AuditLog
            {
                UserId = visit.Id,
                Action = $"Retrieving Visitor Name: {visit.FirstName + "" + visit.LastName} with this email {visit.EmailAddress}",
                Timestamp = DateTime.Now,
            };
            await _auditLog.CreateAsync(auditLog);
            await _auditLog.SaveAsync();
            return new BaseResponse<VisitorDto>
            {
                Status = true,
                Message = "Visitor Successfully Retrieved",
                Data = new VisitorDto
                {
                    EmailAddress = visit.EmailAddress,
                    FirstName = visit.FirstName,
                    LastName = visit.LastName,
                    PhoneNumber = visit.PhoneNumber,
                    HostEmail = visit.HostEmail,
                    Image = visit.Image,
                    Gender = visit.Gender.ToString(),
                    Visits = visit.Visits.Select(x => new VisitDto
                    {
                        VisitDate = x.VisitDate,
                        VisitorId = x.VisitorId,
                        VisitTime = x.VisitTime,
                        VisitReason = x.VisitReason.ToString(),
                        VisitStatus = x.VisitStatus.ToString(),
                    }).ToList(),
                }
            };
        }

        


        public async Task<BaseResponse<VisitorDto>> Register(VisitorRequestModel model, VisitRequestModel visitModel)
        {
            var host = await _user.Get(x => x.Email == model.HostEmail);

            if (host == null)
            {
                return new BaseResponse<VisitorDto>
                {
                    Message = $"Host not found: {model.HostEmail}",
                    Status = false,
                };
            }

            string fileName = null;
            if (model.Image != null)
            {
                fileName = SaveVisitorImage(model.Image);
            }

            if (!IsValidEmail(model.EmailAddress))
            {
                return new BaseResponse<VisitorDto>
                {
                    Status = false,
                    Message = $"Invalid email format: {model.EmailAddress}.",
                };
            }

            var visitor = new Visitor
            {
                
                EmailAddress = model.EmailAddress,
                PhoneNumber = model.PhoneNumber,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Image = fileName,
                HostEmail = model.HostEmail,
                Gender = (Gender)Enum.ToObject(typeof(Gender), model.Gender),
                DateCreated = DateTime.Now,
            };
            
            var visit = new Visit
            {
                User = host,
                UserId = host.Id,
                Visitor = visitor,
                VisitorId = visitor.Id,
                VisitDate = visitModel.VisitDateAndTime,
                VisitTime = visitModel.VisitDateAndTime,
                VisitReason = visitModel.VisitReason,
                VisitStatus = VisitStatus.Pending,
                DateCreated = DateTime.Now,
            };

            

            SendVisitRequestEmail(host, visitor, visit);


            await SaveEntitiesAndCreateAuditLog( visitor, visit, host);

            SendVisitorEmail(visitor, visit);

            return new BaseResponse<VisitorDto>
            {
                Status = true,
                Message = "Visit Request Sent Successfully",
                Data = new VisitorDto
                {
                    EmailAddress = visitor.EmailAddress,
                    FirstName = visitor.FirstName,
                    LastName = visitor.LastName,
                    PhoneNumber = visitor.PhoneNumber,
                    HostEmail = visitor.HostEmail,
                    Image = visitor.Image,
                }
            };
        }

        

        public async Task<BaseResponse<VisitorDto>> GetVisit(string visitId)
        {
            var visit = await _visit.Get(visitId);

            if (visit == null)
            {
                return new BaseResponse<VisitorDto>
                {
                    Message = "Visit is not found",
                    Status = false,
                };
            }
            var auditLog = new AuditLog()
            {
               Timestamp = DateTime.Now,
                UserId = visit.UserId,
                 DateCreated = DateTime.Now,
                  Action = $"Getting Visit Of this Visit {visitId}",
            };

            return new BaseResponse<VisitorDto>
            {
                Message = "Visit Found Successfully",
                Status = true,
                 Data =  new VisitorDto()
                 {
                     EmailAddress = visit.Visitor.EmailAddress,
                     FirstName= visit.Visitor.FirstName,
                     HostEmail= visit.Visitor.HostEmail,
                     PhoneNumber= visit.Visitor.PhoneNumber, 
                     Image = visit.Visitor.Image,
                     LastName = visit.Visitor.LastName,
                     Gender = visit.Visitor.Gender.ToString(),
                     VisitDate = visit.VisitDate,
                     VisitTime = visit.VisitTime,    
                       Id = visit.Id,

                 }
            };
        }


        public async Task<BaseResponse<VisitDto>> ApproveRequest(string VisitId)
        {
            var visit = await _visit.Get(VisitId);

            if (visit == null || visit.VisitStatus != VisitStatus.Pending)
            {
                return new BaseResponse<VisitDto>
                {
                    Status = false,
                    Message = "Visit not found or cannot be approved.",
                };
            }

            string receiverEmail = visit.Visitor.EmailAddress;
            string receiverName = $"{visit.Visitor.FirstName} {visit.Visitor.LastName}";
            string subject = " SecureGuard Notification";
            string message = $"<html><body><h2> Approve Request</h2></body></html>\n" +
                             $"<html><body><h3>{visit.Visitor.HostEmail} Approve Your Visit Request </h3></body></html> :\n" +
                             $"<html><body><h4> Your Visit QR-Code </h4></body></html>";

            var emailDto = new EMailDto
            {
                ToEmail = receiverEmail,
                ToName = receiverName,
                Subject = subject,
                HtmlContent = message,

            };

            _email.SendEMail(emailDto);

            string qrCodeContent = $"VisitId: {visit.Id}, VisitStatus: Approved";

            var qrCodeWriter = new ZXing.BarcodeWriterPixelData
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 300,
                    Height = 600,
                    Margin = 0
                }
            };

            var pixelData = qrCodeWriter.Write(qrCodeContent);

            string wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "QRcodes");
            bool wwwrootPathExists = Directory.Exists(wwwrootPath);

            if (!wwwrootPathExists)
            {
                Directory.CreateDirectory(wwwrootPath);
            }

            string qrCodeFileName = $"{VisitId}_QRCode.png";
            string qrCodeFilePath = Path.Combine(wwwrootPath, qrCodeFileName);

            using (var bitmap = new Bitmap(pixelData.Width, pixelData.Height))
            {
                var bitmapData = bitmap.LockBits(new Rectangle(0, 0, pixelData.Width, pixelData.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                try
                {
                    Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
                }
                finally
                {
                    bitmap.UnlockBits(bitmapData);
                }

                bitmap.Save(qrCodeFilePath, ImageFormat.Png);
            }

            string receiverEmails = visit.Visitor.EmailAddress;
            string receiverNames = $"{visit.Visitor.FirstName} {visit.Visitor.LastName}";
            string subjects = "SecureGuard Notification";
            string messages = $"<html><body>" +
                 $"<h2> Approve Request</h2>" +
                 $"<h3>{visit.Visitor.HostEmail} Accept Your Visit Request </h3> :\n" +
                 $"<h4>QR Code:</h4>" +
                 $"<img src='cid:qrcode' alt='QR Code'>" +
                 $"</body></html>";

            var linkedResource = new LinkedResource(qrCodeFilePath, "image/png")
            {
                ContentId = "qrcode",
                TransferEncoding = TransferEncoding.Base64
            };

            var view = AlternateView.CreateAlternateViewFromString(message, null, MediaTypeNames.Text.Html);
            view.LinkedResources.Add(linkedResource);

            var QRCodeEmailDto = new EMailDto
            {
                ToEmail = receiverEmails,
                ToName = receiverNames,
                Subject = subjects,
                AlternateViews = new List<AlternateView> { view }
            };
            string qrCodeImagePath = Path.Combine("wwwroot", "QRcodes", qrCodeFileName);
            _email.QRCodeEMail(QRCodeEmailDto, qrCodeImagePath);

            var auditLog = new AuditLog
            {
                Action = $"Visit Approved: {VisitId}",
                Timestamp = DateTime.Now,
            };
            await _auditLog.CreateAsync(auditLog);
            await _auditLog.SaveAsync();

            visit.VisitStatus = VisitStatus.Approved;

            _visit.Update(visit);
            await _visit.SaveAsync();

            return new BaseResponse<VisitDto>
            {
                Status = true,
                Message = "Visit Approved Successfully",
            };
        }


        public async Task<BaseResponse<VisitDto>> DeniedRequest(string VisitId)
        {
            var visit = await _visit.Get(VisitId);

            if (visit == null || visit.VisitStatus != VisitStatus.Pending)
            {
                return new BaseResponse<VisitDto>
                {
                    Status = false,
                    Message = "Visit not found or cannot be denied.",
                };
            }


            visit.VisitStatus = VisitStatus.Denied;
            _visit.Update(visit);
            await _visit.SaveAsync();

            string receiverEmail = visit.Visitor.EmailAddress;
            string receiverName = $"{visit.Visitor.FirstName} {visit.Visitor.LastName}";
            string subject = " SecureGuard Notification";
            string message = $"<html><body><h2> Denied Request</h2></body></html>\n" +
                             $"<html><body><h3>{visit.Visitor.HostEmail} Denied Your Visit Request </h3></body></html> :\n"+
                             $"<html><body><h4> Try Again Later Or Contact Your Host To Approve Your Request</h4></body></html>";

            var emailDto = new EMailDto
            {
                ToEmail = receiverEmail,
                ToName = receiverName,
                Subject = subject,
                HtmlContent = message,
                
            };

            _email.SendEMail(emailDto);

            var auditLog = new AuditLog
            {
                Action = $"Visit Denied: {VisitId}",
                Timestamp = DateTime.Now,
            };
            await _auditLog.CreateAsync(auditLog);
            await _auditLog.SaveAsync();

            return new BaseResponse<VisitDto>
            {
                Status = true,
                Message = "Visit Denied Successfully",
            };
        }


        private string SaveVisitorImage(IFormFile image)
        {
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "CredentialPictures");
            bool basePathExists = Directory.Exists(basePath);

            if (!basePathExists)
            {
                Directory.CreateDirectory(basePath);
            }

            var fileName = Path.GetFileName(image.FileName);
            var filePath = Path.Combine(basePath, fileName);

            if (!File.Exists(filePath) && IsImageFileExtension(fileName))
            {
                using var stream = new FileStream(filePath, FileMode.Create);
                image.CopyTo(stream);
            }

            return fileName;
        }

        private bool IsImageFileExtension(string fileName)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return allowedExtensions.Contains(extension);
        }

        private async Task SaveEntitiesAndCreateAuditLog(Visitor visitor, Visit visit, User host)
        {
            await _visitor.CreateAsync(visitor);
            await _visit.CreateAsync(visit);

            await _visitor.SaveAsync();
            await _visit.SaveAsync();

            var auditLog = new AuditLog
            {
                UserId = host.Id,
                Action = $"Creating Visitor Request for {visitor.FirstName} {visitor.LastName}",
                Timestamp = DateTime.Now,
            };
            

            await _auditLog.CreateAsync(auditLog);
            await _auditLog.SaveAsync();
        }




        // Email Implementation
        private void SendVisitRequestEmail(User host, Visitor visitor, Visit visit)
        {
            string receiverEmail = visitor.HostEmail;
            string receiverName = $"{host.Profile.FirstName} {host.Profile.LastName}";
            string visitId = visit.Id;
            string token = _tokenService.GenerateApproveToken().GetAwaiter().GetResult();
            string approveUrl = $"http://127.0.0.1:5505/Approve.html?token={token}&visitId={visitId}";
            string rejectUrl = $"http://127.0.0.1:5505/Reject.html?token={token}&visitId={visitId}";
            string subject = "SecureGuard Guest Notification";

            string imageBase64 = Convert.ToBase64String(File.ReadAllBytes(Path.Combine("wwwroot", "CredentialPictures", visitor.Image)));
            string imageHtml = $"<img src='data:image/png;base64,{imageBase64}' alt='GuestImage'>";

            string message = $@"
<html>
    <body>
        <h2>A Guest Request Kindly Approve Or Denied</h2>
        <h3>The Guest Details:</h3>
        <h4>GuestName: {visitor.FirstName} {visitor.LastName}</h4>
        <h4>GuestEmail: {visitor.EmailAddress}</h4>
        <h4>GuestPhoneNumber: {visitor.PhoneNumber}</h4>
        <h4>GuestGender: {visitor.Gender}</h4>
        {imageHtml}
        <h4>VisitReason: {visit.VisitReason}</h4>
        <h4>VisitTime: {visit.VisitTime.TimeOfDay}</h4>
        <h4>VisitDate: {visit.VisitDate.Date}</h4>
        <a href=""{approveUrl}"" style=""background-color: #4CAF50; color: white; padding: 10px; text-align: center; text-decoration: none; display: inline-block; border-radius: 5px;"">Approve</a>
        <a href=""{rejectUrl}"" style=""background-color: #f44336;  color: white; padding: 10px; text-align: center; text-decoration: none; display: inline-block; border-radius: 5px; margin-left: 10px;"">Reject</a>
    </body>
</html>";

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
            const string emailRegex = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, emailRegex);
        }

        private void SendVisitorEmail(Visitor visitor, Visit visit)
        {
            string receiverEmail = visitor.EmailAddress;
            string receiverName = $"{visitor.FirstName} {visitor.LastName}";
            string subject = " SecureGuard Guest Notification";
            string message = $"<html><body><h2>Your Visit Request is Processing</h2></body></html>\n" +
                             $"<html><body><h3>Your Details</h3></body></html> :\n" +
                             $"<html><body><h4>Name: {visitor.FirstName} {visitor.LastName}</h4></body></html>\n" +
                             $"<html><body><h4>Email: {visitor.EmailAddress}</h4></body></html>\n" +
                             $"<html><body><h4>PhoneNumber: {visitor.PhoneNumber}</h4></body></html>\n" +
                             $"<html><body><h4>Gender: {visitor.Gender}</h4></body></html>";
                            

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
