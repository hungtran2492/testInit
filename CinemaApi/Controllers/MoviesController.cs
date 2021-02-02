using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CinemaApi.Data;
using CinemaApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private CinemaDbContext _dbContext;

        public MoviesController(CinemaDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        // GET: api/Movies
        [HttpGet]
        public IActionResult Get()
        {
            //return _dbContext.Movies;
            return Ok(_dbContext.Movies);
            //return StatusCode(StatusCodes.Status200OK);
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var movie = _dbContext.Movies.Find(id);
            if(movie == null)
            {
                return NotFound("No record found against this Id");
            }
            return Ok(movie);
        }

        [HttpGet("{id}")]
        [HttpGet("[action]/{id}")]
        public int Test(int id)
        {
            return id;
        }

        // POST: api/Movies
        //[HttpPost]
        //public IActionResult Post([FromBody] Movie movieObj)
        //{
        //    _dbContext.Movies.Add(movieObj);
        //    _dbContext.SaveChanges();
        //    return StatusCode(StatusCodes.Status201Created);
        //}

        [HttpPost]
        public IActionResult Post([FromForm] Movie movieObj)
        {
            var guid = Guid.NewGuid();
            var filePath =  Path.Combine("wwwroot",guid+".jpg");
            if (movieObj.Image != null)
            {
                var fileStreeam = new FileStream(filePath, FileMode.Create);
                movieObj.Image.CopyTo(fileStreeam);
            }
            movieObj.ImageUrl = filePath.Remove(0,7);
            _dbContext.Movies.Add(movieObj);
            _dbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);

        }

        // PUT: api/Movies/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromForm] Movie movieObj)
        {
           var movie =  _dbContext.Movies.Find(id);
            if(movie == null)
            {
                return NotFound("No record found against this Id");
            } else
            {

                var guid = Guid.NewGuid();
                var filePath = Path.Combine("wwwroot", guid + ".jpg");
                if (movieObj.Image != null)
                {
                    var fileStreeam = new FileStream(filePath, FileMode.Create);
                    movieObj.Image.CopyTo(fileStreeam);
                    movie.ImageUrl = filePath.Remove(0, 7);
                }
                 
                movie.Name = movieObj.Name;
                movie.Language = movieObj.Language;
                _dbContext.SaveChanges();
                return Ok(_dbContext.Movies);
            } 
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var movie = _dbContext.Movies.Find(id);
            if (movie == null)
            {
                return NotFound("No record found against this Id");
            }
            else
            {
                _dbContext.Movies.Remove(movie);
                _dbContext.SaveChanges();
                return Ok(_dbContext.Movies);
            }
           
        }
    }
}
