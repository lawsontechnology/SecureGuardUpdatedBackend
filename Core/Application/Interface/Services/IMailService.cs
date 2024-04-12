using Visitor_Management_System.Core.Application.DTOs;


namespace Visitor_Management_System.Core.Application.Interface.Services
{
    public interface IMailServices
    {
        public void SendEMail(EMailDto mailRequest);
        public void QRCodeEMail(EMailDto mailRequest, string qrCodeImagePath);
    }
}
