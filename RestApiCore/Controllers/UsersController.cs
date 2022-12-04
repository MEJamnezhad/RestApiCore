using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using ServiceLayer;

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

        //[HttpGet]
        //public async Task<IActionResult> Get()
        //{
        //    using (var context = new RestApiDbContext())
        //    {
        //        return Ok(await context.Users.Where(p => p.IsDelete == false).ToListAsync());
        //    }
        //}


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] QueryParameters queryParameters)
        {
            //using (var context = new DatabaseContext())
            //{
            //    IQueryable<User> users = context.Users
            //        .Skip(queryParameters.Size * (queryParameters.Page - 1))
            //        .Take(queryParameters.Size);
            //    return Ok(await users.ToListAsync());
            //}

            var users = ServiceLayer.User.GetAllNonDeleted(queryParameters);

            return Ok(await users);

        }

        [HttpGet("all")]
        public async Task<ActionResult<User>> GetAll()
        {
            //using (var context = new DatabaseContext())
            //{
            //    return Ok(await context.Users.ToListAsync());
            //}

            //var users = ;
            return Ok(await ServiceLayer.User.GetAllNonDeleted());

        }





        // All below items are same (http get action and route template
        [HttpGet("{Id}")]

        //1-  [HttpGet]
        //    [Route("api/[controller]/{id}")]

        //2-  [HttpGet]
        //    [Route("{id}")]

        //3-  [HttpGet,Route("api/[controller]/{id}")]

        //4-  [HttpGet,Route("{id}")]
        public async Task<IActionResult> Get(int Id)
        {
            using (var context = new DatabaseContext())
            {
                var user = await context.Users.FindAsync(Id);
                if (user != null)
                {
                    return Ok(user);
                }
                return NotFound();
            }
        }



        [HttpGet("Details")]
        public async Task<IActionResult> GetDetails()
        {
            using (var context = new DatabaseContext())
            {
                return Ok(await context.Users.Where(p => p.IsDeleted == false).Select(p => new
                {

                    p.Id,
                    p.Email,
                    p.FirstName,
                    p.LastName,
                    p.Mobile,
                    p.BirthDay,

                }).ToListAsync());
            }
        }



        [HttpGet("Role/{role}")]
        public async Task<IActionResult> GetRole(int role)
        {
            using (var context = new DatabaseContext())
            {
                var user = await context.Users.Where(p => (int)p.Role == role && p.IsDeleted == false).Select(p => new
                {
                    p.Id,
                    p.Email,
                    p.FirstName,
                    p.LastName,
                    p.Mobile,
                    p.BirthDay,
                }).ToListAsync();
                if (user != null)
                {
                    return Ok(user);
                }
                return NotFound();
            }
        }


        [HttpGet("Email/{Email}")]
        public async Task<IActionResult> Get(string Email)
        {
            using (var context = new DatabaseContext())
            {
                var user = await context.Users.Where(p => p.Email.Contains(Email) && p.IsDeleted == false).ToListAsync();
                if (user != null)
                {
                    return Ok(user);
                }
                return NotFound();
            }
        }



        //    URL:   localhost:5186/api/users/search/{First Name Value}/{Last Name Value}
        [HttpGet("search/{firstname}/{lastname}")]
        public async Task<IActionResult> Get(string firstname, string lastname)
        {
            using (var context = new DatabaseContext())
            {
                var users = await context.Users.Where(p => (p.FirstName.Contains(firstname) || p.LastName.Contains(lastname)) && p.IsDeleted == false).ToListAsync();
                if (users.Count > 0)
                {
                    return Ok(users);
                }
                return NotFound();
            }
        }


        //    URL:   localhost:5186/api/users/search2?firstname={First Name Value}&lastname={Last Name Value}
        [HttpGet("search")]
        public async Task<IActionResult> GetByFirstnameAndLastname([FromQuery] string firstname, [FromQuery] string lastname)
        {
            using (var context = new DatabaseContext())
            {
                var users = await context.Users.Where(p => (p.FirstName.Contains(firstname) || p.LastName.Contains(lastname)) && p.IsDeleted == false).ToListAsync();
                if (users.Count > 0)
                {
                    return Ok(users);
                }
                return NotFound();
            }
        }

        //    URL:      ~/api/users/search/{Role Value}?firstname={First Name Value}
        [HttpGet("search/{role}")]
        public async Task<IActionResult> Get(int role, [FromQuery] string firstname)
        {
            using (var context = new DatabaseContext())
            {
                var users = await context.Users.Where(p => (int)p.Role == role && p.FirstName.Contains(firstname) && p.IsDeleted == false).ToListAsync();
                if (users.Count > 0)
                {
                    return Ok(users);
                }
                return NotFound();
            }
        }



        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            if (ModelState.IsValid)
            {
                using (var context = new DatabaseContext())
                {
                    try
                    {
                        context.Users.Add(user);
                        await context.SaveChangesAsync();

                        return Created("The user added", user);  // With message (Show message in value of Location HTTP Header Key)
                        // Or
                        //return CreatedAtAction("get", new {id = user.Id},user);  // Without message and call get method with input value (user.id)
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("Exception", ex.Message);
                        return StatusCode(500, ModelState);
                    }
                }
            }



            return BadRequest("The Data is wrong");
        }


        [HttpPut]
        public async Task<IActionResult> UpdateUser(User user)
        {
            using (var context = new DatabaseContext())
            {
                bool exist = context.Users.Any(p => p.Id == user.Id);
                if (exist == true)
                {
                    context.Entry(user).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                    return Ok(user);
                    // Or   return Ok();  Or    return NoContent(); 
                }
                return BadRequest();
            }
        }

        [HttpPatch("{Id}")]   // Or
        //[HttpPut("{Id}")]
        public async Task<IActionResult> ChangeActivation(int Id)
        {
            using (var context = new DatabaseContext())
            {
                var updateuser = context.Users.Find(Id);
                if (updateuser != null)
                {
                    updateuser.IsActie = !updateuser.IsActie;
                    await context.SaveChangesAsync();
                    return Ok();
                    // Or return Ok(updateuser);  Or return NoContent();
                }
                return BadRequest();
            }
        }


        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteUser(int Id)
        {
            using (var context = new DatabaseContext())
            {
                var updateuser = context.Users.Find(Id);
                if (updateuser != null)
                {
                    updateuser.IsDeleted = true;
                    await context.SaveChangesAsync();
                    return Ok();
                    // Or return Ok(updateuser);  Or  return NoContent(); 
                }
                return BadRequest();
            }
        }


        //-- This method delete user data from database.
        //-- due to the fact that delete rule is "cascade",
        //-- it also delete all related data in others tables.

        //[HttpDelete("{Id}")]
        //public IActionResult DeleteUser(int Id)
        //{
        //    using (var context = new RestApiDbContext())
        //    {
        //        var updateuser = context.Users.Find(Id);
        //        if (updateuser != null)
        //        {
        //            context.Users.Remove(updateuser);
        //            context.SaveChanges();
        //            return NoContent();
        //        }
        //        return BadRequest();
        //    }
        //}


    }
}
