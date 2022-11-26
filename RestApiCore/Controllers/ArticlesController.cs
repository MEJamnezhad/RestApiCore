using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RestApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {


        [HttpGet]
        public IActionResult Get()
        {
            
            return Ok();
        }







        [HttpPost("Uploadfile")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
        {
            string filename = Guid.NewGuid().ToString();
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\", filename);
            FileStream fs = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(fs);
            fs.Close();
            return Created("", null);
        }
    }
}
