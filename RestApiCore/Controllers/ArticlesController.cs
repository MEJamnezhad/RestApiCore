using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer;
using static System.Net.Mime.MediaTypeNames;

namespace RestApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] QueryParameters queryParameters)
        {
            return Ok(await ServiceLayer.Article.GetAllActive(queryParameters));
        }


        [HttpGet("all")]
        public async Task<ActionResult<Article>> GetAll()
        {
            return Ok(await ServiceLayer.Article.GetAllNonDeleted());
        }

        [HttpGet("alld")]
        public async Task<ActionResult<Article>> GetAllDeleted()
        {
            return Ok(await ServiceLayer.Article.GetAllDeleted());
        }

        [HttpGet("cat/{Id}")]
        public async Task<ActionResult<Article>> GetAllArticleByCategory(int Id)
        {
            return Ok(await ServiceLayer.Article.GetActiveArticles(Id));
        }

        [HttpGet("cat/{Id}/{top}")]
        public async Task<ActionResult<Article>> GetTopArticleByCategory(int Id, int top)
        {
            return Ok(await ServiceLayer.Article.GetTop(Id, top));
        }

        [HttpGet("cat/top/{top}")] //  [HttpGet("cat/0/{top}")]
        public async Task<ActionResult<Article>> GetTopArticle(int top)
        {
            return Ok(await ServiceLayer.Article.GetTop(0, top));
        }


        [HttpGet("{Id}")]
        public async Task<IActionResult> GetActiveArticle(int Id)
        {
            Article article = await ServiceLayer.Article.Get(Id);
            if (article != null && article.IsActive == true)
            {
                return Ok(article);
            }
            return NotFound();
        }


        [HttpGet("act/{Id}")]
        public async Task<IActionResult> GetArticle(int Id)
        {
            Article article = await ServiceLayer.Article.Get(Id);
            if (article != null)
            {
                return Ok(article);
            }
            return NotFound();
        }


        //    URL:  ~/api/Articles/search/{Text}
        [HttpGet("search/{text}")]
        public async Task<IActionResult> Get(string text)
        {
            var Articles = await ServiceLayer.Article.FindAll(p => (p.Text.Contains(text) || p.Title.Contains(text)) && p.IsDeleted == false && p.IsActive == true);
            if (Articles.Count > 0)
            {
                return Ok(Articles);
            }
            return NotFound();
        }


        //    URL:   localhost:5186/api/Articles/search2?text={text Value}
        [HttpGet("search")]
        public async Task<IActionResult> GetByFirstnameAndLastname([FromQuery] string text)
        {
            var Articles = await ServiceLayer.Article.FindAll(p => (p.Text.Contains(text) || p.Title.Contains(text)) && p.IsDeleted == false);
            if (Articles.Count > 0)
            {
                return Ok(Articles);
            }
            return NotFound();
        }


        [HttpGet("general/{top}")]
        public async Task<IActionResult> GetTopics(int top = 0)
        {
            return Ok(ServiceLayer.Article.GetTop(top).Result.Select(p => new
            {
                p.Id,
                p.Title,
                p.Text,
                p.Picture,
                p.Visit,
                p.CreationDate,
                p.LastUpdateDate
            }));
        }

        [HttpGet("general/{categoryId}/{top}")]
        public async Task<IActionResult> GetTopics(int categoryId, int top = 0)
        {
            return Ok(ServiceLayer.Article.GetTop(categoryId, top).Result.Select(p => new
            {
                p.Id,
                p.Title,
                p.Text,
                p.Picture,
                p.Visit,
                p.CreationDate,
                p.LastUpdateDate
            }));
        }




        [HttpPost]
        public async Task<IActionResult> AddArticle(Article article)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool added = await ServiceLayer.Article.Insert(article);
                    if (added)
                    {
                        return Created("The Article added", article);  // With message (Show message in value of Location HTTP Header Key)
                        // Or
                        //return CreatedAtAction("get", new {id = article.Id},article);  // Without message and call get method with input value (Article.id)
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
        public async Task<IActionResult> UpdateArticle(Article article)
        {
            bool changed = await ServiceLayer.Article.Update(article, article.Id);
            if (changed == true)
            {
                return Ok(article);
                // Or   return Ok();  Or    return NoContent(); 
            }
            return BadRequest();
        }


        [HttpPatch("{Id}")]   // Or
                              //[HttpPut("{Id}")]
        public async Task<IActionResult> ChangeActivation(int Id)
        {
            bool changed = await ServiceLayer.Article.ChangeActivation(Id);
            if (changed == true)
            {
                return Ok();
                //   Or  return NoContent(); 
            }
            return BadRequest();
        }


        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteArticle(int Id)
        {
            bool deleted = await ServiceLayer.Article.Delete(Id);
            if (deleted == true)
            {
                return Ok();
                //   Or  return NoContent(); 
            }
            return BadRequest();
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
