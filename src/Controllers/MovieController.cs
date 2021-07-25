using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AlkemyChallenge.Data;
using AlkemyChallenge.Models;
using AlkemyChallenge.Repositories;
using AlkemyChallenge.Services;

namespace AlkemyChallenge.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly MovieRepository _movieRepository;
        private readonly AppDbContext _context;
        private readonly FileService _fileService;
        public MovieController(MovieRepository movieRepository, FileService fileService,AppDbContext context)
        {
            _movieRepository = movieRepository;
            _fileService = fileService;
            _context = context;
        }

        // GET: api/Movie
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            var movies = await _movieRepository.GetAll();
            return movies.ToList();
        }

        //// GET: api/Movie/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            var movie = await _movieRepository.GetById(id);

            if (movie == null)
            {
                return NotFound();
            }

            return movie;
        }

        //// POST: api/Movie
        [HttpPost]
        public async Task<ActionResult> PostMovie([FromForm] Movie movie, [FromForm] IFormFile image)
        {
            movie.CreatedAt = DateTimeOffset.Now;
            movie.Image = await _fileService.SaveImage(image);
            //await _movieRepository.Add(movie);

            return CreatedAtAction("GetMovie", new { id = movie.Id }, movie);
        }

        //// PUT: api/Movie/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutMovie(int id, Movie movie)
        //{
        //    if (id != movie.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(movie).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!MovieExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}


        //// DELETE: api/Movie/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteMovie(int id)
        //{
        //    var movie = await _context.Movies.FindAsync(id);
        //    if (movie == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Movies.Remove(movie);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool MovieExists(int id)
        //{
        //    return _context.Movies.Any(e => e.Id == id);
        //}
    }
}
