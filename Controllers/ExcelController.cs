using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Visitor_Management_System.Core.Application.Interface.Services;
using System.Security.Claims;
using Visitor_Management_System.Core.Application.DTOs;

namespace Visitor_Management_System.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ExcelController : ControllerBase
    {
        private readonly IExcelService _excelService;

        public ExcelController( IExcelService excelService)
        {
            _excelService = excelService;
        }

        [HttpGet("Template")]
        public  IActionResult ExportExcelTemplate()
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (var wb = new XLWorkbook())
                    {
                        var empSheet = wb.AddWorksheet("User Records");

                        var headers = new[] { "S/N", "Email", "FirstName", "LastName", "PhoneNumber" };
                        for (int i = 0; i < headers.Length; i++)
                        {
                            empSheet.Cell(1, i + 1).Value = headers[i];
                        }

                        wb.SaveAs(ms);
                    }

                    HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                    return new FileContentResult(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = "SecureGuard_Template.xlsx"
                          
                    };
                }
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error exporting Excel template: {ex.Message}");
                return new BadRequestResult();
            }
        }

       /* [Authorize(Roles = "Admin")]*/
        [HttpPost("Upload")]
         public async Task<IActionResult> Upload([FromForm] ExcelRequestModel model)
         {
            /*var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;*/
            var userEmail = "lawalola04@gmail.com";
            var result = await _excelService.ImportAndSave(model,userEmail);
            if (result.Status)
            {
                return Ok(result);
            }

            return BadRequest(result.Message);
         }
    }




}
