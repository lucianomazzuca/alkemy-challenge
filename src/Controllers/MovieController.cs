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
using AlkemyChallenge.Exceptions;
using AutoMapper;
using AlkemyChallenge.DTOs.Movie;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using Microsoft.Extensions.Primitives;

namespace AlkemyChallenge.Controllers
{
    [Authorize]
    [Route("api/movies")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly MovieRepository _movieRepository;
        private readonly FileService _fileService;
        private readonly IMapper _mapper;

        public MovieController(MovieRepository movieRepository, FileService fileService, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _fileService = fileService;
            _mapper = mapper;
        }

        // GET: api/Movie
        [HttpGet]
        public ActionResult<IEnumerable<MovieReadDto>> GetMovies([FromQuery] string title, [FromQuery] string order, [FromQuery] int? genre = null)
        {
            var movies = _movieRepository.GetAllWith(title, order, genre);
            var moviesReadDto = _mapper.Map<IEnumerable<MovieReadDto>>(movies);
            return Ok(moviesReadDto);
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

            var movieDto = _mapper.Map<MovieDetailDto>(movie);
            return Ok(movieDto);
        }

        //// POST: api/Movie
        [HttpPost]
        public async Task<ActionResult> PostMovie([FromForm] Movie movie, [FromForm] IFormFile image)
        {
            movie.CreatedAt = DateTimeOffset.Now;

            if (image != null)
            {
                movie.Image = await _fileService.SaveImage(image);
            }

            var form = HttpContext.Request.Form;
            int[] charactersIds = null;
            int[] genresIds = null;

            if (!StringValues.IsNullOrEmpty(form["characters"]))
            {
                charactersIds = JsonSerializer.Deserialize<int[]>(form["characters"]);
            }

            if (!StringValues.IsNullOrEmpty(form["genres"]))
            {
                genresIds = JsonSerializer.Deserialize<int[]>(form["genres"]);
            }

            await _movieRepository.AddWith(movie, charactersIds, genresIds);

            return StatusCode(201);
        }

        //// PUT: api/Movie/5
        [HttpPut("{id}")]
        public async Task<ActionResult> PutMovie (int id, [FromForm] Movie movie, [FromForm] IFormFile image)
        {
            if (image != null)
            {
                movie.Image = await _fileService.SaveImage(image);
            }

            movie.Id = id;

            var form = HttpContext.Request.Form;
            int[] charactersIds = null;
            int[] genresIds = null;

            if (!StringValues.IsNullOrEmpty(form["characters"]))
            {
                charactersIds = JsonSerializer.Deserialize<int[]>(form["characters"]);
            }

            if (!StringValues.IsNullOrEmpty(form["genres"]))
            {
                genresIds = JsonSerializer.Deserialize<int[]>(form["genres"]);
            }

            await _movieRepository.UpdateWith(movie, charactersIds, genresIds);
            return NoContent();
        }

        //// DELETE: api/Movie/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMovie(int id)
        {
            try
            {
                await _movieRepository.Delete(id);

            } catch(RecordNotFoundException)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
