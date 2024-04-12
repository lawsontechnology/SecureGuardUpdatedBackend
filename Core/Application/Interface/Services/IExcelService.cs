using Microsoft.AspNetCore.Mvc;
using Visitor_Management_System.Core.Application.DTOs;

namespace Visitor_Management_System.Core.Application.Interface.Services
{
    public interface IExcelService
    {
        Task<BaseResponse<IEnumerable<UserDto>>> ImportAndSave(ExcelRequestModel model, string userEmail);
    }
}
