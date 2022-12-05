using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer;

namespace RestApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] QueryParameters queryParameters)
        {
            return Ok(await ServiceLayer.Category.GetAllActive(queryParameters));
        }


        [HttpGet("all")]
        public async Task<ActionResult<Category>> GetAll()
        {
            return Ok(await ServiceLayer.Category.GetAllNonDeleted());
        }

        [HttpGet("alld")]
        public async Task<ActionResult<Category>> GetAllDeleted()
        {
            return Ok(await ServiceLayer.Category.GetAllDeleted());
        }


        [HttpGet("{Id}")]
        public async Task<IActionResult> GetActiveCategory(int Id)
        {
            Category category = await ServiceLayer.Category.Get(Id);
            if (category != null && category.IsActive == true)
            {
                return Ok(category);
            }
            return NotFound();
        }


        [HttpGet("act/{Id}")]
        public async Task<IActionResult> GetCategory(int Id)
        {
            Category category = await ServiceLayer.Category.Get(Id);
            if (category != null)
            {
                return Ok(category);
            }
            return NotFound();
        }


        //    URL:  ~/api/Categorys/search/{Text}
        [HttpGet("search/{text}")]
        public async Task<IActionResult> Get(string text)
        {
            var categorys = await ServiceLayer.Category.FindAll(p => p.Name.Contains(text) && p.IsDeleted == false);
            if (categorys.Count > 0)
            {
                return Ok(categorys);
            }
            return NotFound();
        }


        //    URL:   localhost:5186/api/Categorys/search2?text={text Value}
        [HttpGet("search")]
        public async Task<IActionResult> GetByFirstnameAndLastname([FromQuery] string text)
        {
            var categorys = await ServiceLayer.Category.FindAll(p => p.Name.Contains(text) && p.IsDeleted == false);
            if (categorys.Count > 0)
            {
                return Ok(categorys);
            }
            return NotFound();
        }



        [HttpPost]
        public async Task<IActionResult> AddCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool added = await ServiceLayer.Category.Insert(category);
                    if (added)
                    {
                        return Created("The Category added", category);  // With message (Show message in value of Location HTTP Header Key)
                        // Or
                        //return CreatedAtAction("get", new {id = Category.Id},Category);  // Without message and call get method with input value (Category.id)
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Exception", ex.Message);
                    return StatusCode(500, ModelState);
                }

            }
            return BadRequest("The Data is wrong");
        }


        [HttpPut]
        public async Task<IActionResult> UpdateCategory(Category category)
        {
            bool changed = await ServiceLayer.Category.Update(category, category.Id);
            if (changed == true)
            {
                return Ok(category);
                // Or   return Ok();  Or    return NoContent(); 
            }
            return BadRequest();
        }


        [HttpPatch("{Id}")]   // Or
                              //[HttpPut("{Id}")]
        public async Task<IActionResult> ChangeActivation(int Id)
        {
            bool changed = await ServiceLayer.Category.ChangeActivation(Id);
            if (changed == true)
            {
                return Ok();
                //   Or  return NoContent(); 
            }
            return BadRequest();
        }


        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteCategory(int Id)
        {
            bool deleted = await ServiceLayer.Category.Delete(Id);
            if (deleted == true)
            {
                return Ok();
                //   Or  return NoContent(); 
            }
            return BadRequest();
        }


    }
}
