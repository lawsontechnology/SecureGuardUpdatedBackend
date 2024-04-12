using Microsoft.AspNetCore.Mvc;

namespace Visitor_Management_System.Controllers
{
    [ApiController]
    [Route("api/images")]
    public class ImagesController : ControllerBase
    {
        [HttpGet("filename")]
        public IActionResult GetImage([FromQuery]string filename)
        {
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "CredentialPictures", filename);

            if (System.IO.File.Exists(imagePath))
            {
                var imageBytes = System.IO.File.ReadAllBytes(imagePath);
                return File(imageBytes, "image/jpeg"); 
            }
            else
            {
                return NotFound();
            }
        }
    }

}
