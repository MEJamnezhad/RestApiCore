using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestApiCore.Model;

namespace RestApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
      //  [EnableCors]
        [HttpGet]
        public IActionResult Get()
        {
            using (var context = new RestApiDbContext())
            {
                var cats = context.Categories.ToList();
                return Ok(cats);

            }
        }


        [HttpGet("{Id}")]
        public IActionResult Get(int Id)
        {
            using (var context = new RestApiDbContext())
            {
                var cat = context.Categories.Find(Id);
                if (cat != null)
                {
                    return Ok(cat);
                }
                return NotFound();
            }
        }


        [HttpPost]
        public IActionResult AddPosition(Category cat)
        {
            if (ModelState.IsValid)
            {
                using (var context = new RestApiDbContext())
                {
                    context.Categories.Add(cat);
                    context.SaveChanges();
                    return Created("The position added", cat);
                }
            }
            return BadRequest("The Data is wrong");
        }

    }
}
