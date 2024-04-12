using System.Net.Mail;

namespace Visitor_Management_System.Core.Application.DTOs
{
    public class EMailDto
    {
        public string ToEmail { get; set; }
        public string ToName { get; set; }
        public string AttachmentName { get; set; }
        public string HtmlContent { get; set; }
        public string Subject { get; set; }
        public List<AlternateView> AlternateViews { get; set; } = new List<AlternateView>();
    }
}
