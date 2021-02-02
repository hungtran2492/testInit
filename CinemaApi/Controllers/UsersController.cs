using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CinemaApi.Data;
using CinemaApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private CinemaDbContext _dbContext;

        public UsersController(CinemaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public IActionResult Register([FromBody] Users user)
        {
            var userWithSameEmail = _dbContext.Users.Where(u => u.Email == user.Email).SingleOrDefault();

            if (userWithSameEmail != null)
            {
                return BadRequest("User with same email already exists");
            }

            var userObj = new Users
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                Role = "Users"
            };

            _dbContext.Users.Add(userObj);
            _dbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }
    }
}