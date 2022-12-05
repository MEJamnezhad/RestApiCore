using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using ServiceLayer;
using System.Data;
using System.Reflection;

namespace RestApiCore.Controllers
{
    //  [EnableCors("YourNamePolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        //public UsersController()
        //{
        //    _Logger = Logger ?? throw

        //}


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] QueryParameters queryParameters)
        {
            return Ok(await ServiceLayer.User.GetAllNonDeleted(queryParameters));
        }


        [HttpGet("all")]
        public async Task<ActionResult<User>> GetAll()
        {
            return Ok(await ServiceLayer.User.GetAllNonDeleted());
        }

        [HttpGet("alld")]
        public async Task<ActionResult<User>> GetAllDeleted()
        {
            return Ok(await ServiceLayer.User.GetAllDeleted());
        }



        // All below items are same (http get action and route template)

        [HttpGet("{Id}")]

        //1-  [HttpGet]
        //    [Route("api/[controller]/{id}")]

        //2-  [HttpGet]
        //    [Route("{id}")]

        //3-  [HttpGet,Route("api/[controller]/{id}")]

        //4-  [HttpGet,Route("{id}")]
        public async Task<IActionResult> Get(int Id)
        {
            User user = await ServiceLayer.User.Get(Id);
            if (user != null && user.IsDeleted == false)
            {
                return Ok(user);
            }
            return NotFound();

        }



        [HttpGet("Details")]
        public async Task<IActionResult> GetDetails()
        {
            return Ok( ServiceLayer.User.GetAllNonDeleted().Result.Select(p => new
            {
                p.Id,
                p.Email,
                p.FirstName,
                p.LastName,
                p.Mobile,
                p.BirthDay,

            }));
        }



        [HttpGet("Role/{role}")]
        public async Task<IActionResult> GetRole(int role)
        {

            var user = await ServiceLayer.User.FindAll(p => (int)p.Role == role && p.IsDeleted == false);
            if (user != null)
            {
                return Ok(user);
            }
            return NotFound();
        }



        [HttpGet("Email/{Email}")]
        public async Task<IActionResult> Get(string Email)
        {
            var user = await ServiceLayer.User.FindAll(p => p.Email.Contains(Email) && p.IsDeleted == false);
            if (user != null)
            {
                return Ok(user);
            }
            return NotFound();
        }


        //    URL:  ~/api/users/search/{FirstName Value}/{LastName Value}
        [HttpGet("search/{firstname}/{lastname}")]
        public async Task<IActionResult> Get(string firstname, string lastname)
        {

            var users = await ServiceLayer.User.FindAll(p => (p.FirstName.Contains(firstname) || p.LastName.Contains(lastname)) && p.IsDeleted == false);
            if (users.Count > 0)
            {
                return Ok(users);
            }
            return NotFound();

        }


        //    URL:   localhost:5186/api/users/search2?firstname={First Name Value}&lastname={Last Name Value}
        [HttpGet("search")]
        public async Task<IActionResult> GetByFirstnameAndLastname([FromQuery] string firstname, [FromQuery] string lastname)
        {
            var users = await ServiceLayer.User.FindAll(p => (p.FirstName.Contains(firstname) || p.LastName.Contains(lastname)) && p.IsDeleted == false);
            if (users.Count > 0)
            {
                return Ok(users);
            }
            return NotFound();
        }

        //    URL:      ~/api/users/search/{Role Value}?firstname={First Name Value}
        [HttpGet("search/{role}")]
        public async Task<IActionResult> Get(int role, [FromQuery] string firstname)
        {
            var users = await ServiceLayer.User.FindAll(p => (int)p.Role == role && p.FirstName.Contains(firstname) && p.IsDeleted == false);
            if (users.Count > 0)
            {
                return Ok(users);
            }
            return NotFound();
        }



        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool added = await ServiceLayer.User.Insert(user);
                    if (added)
                    {
                        return Created("The user added", user);  // With message (Show message in value of Location HTTP Header Key)
                        // Or
                        //return CreatedAtAction("get", new {id = user.Id},user);  // Without message and call get method with input value (user.id)
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
        public async Task<IActionResult> UpdateUser(User user)
        {
            bool changed = await ServiceLayer.User.Update(user, user.Id);
            if (changed == true)
            {
                return Ok(user);
                // Or   return Ok();  Or    return NoContent(); 
            }
            return BadRequest();
        }


        [HttpPatch("{Id}")]   // Or
                              //[HttpPut("{Id}")]
        public async Task<IActionResult> ChangeActivation(int Id)
        {
            bool changed = await ServiceLayer.User.ChangeActivation(Id);
            if (changed == true)
            {
                return Ok();
                //   Or  return NoContent(); 
            }
            return BadRequest();
        }


        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteUser(int Id)
        {
            bool deleted = await ServiceLayer.User.Delete(Id);
            if (deleted == true)
            {
                return Ok();
                //   Or  return NoContent(); 
            }
            return BadRequest();
        }
      
    }
}

