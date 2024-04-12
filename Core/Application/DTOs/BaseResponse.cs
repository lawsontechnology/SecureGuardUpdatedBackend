namespace Visitor_Management_System.Core.Application.DTOs
{
    public class BaseResponse<T>
    {
        public bool Status { get; set; }

        public string? Message { get; set; }

        public T? Data { get; set; }

        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int  PageSize { get; set;}
                
    }

}
