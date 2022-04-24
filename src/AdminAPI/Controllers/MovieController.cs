using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using MovieTheaterCore.Models;
using Microsoft.EntityFrameworkCore;
using MovieTheaterCore.Services;
using Microsoft.AspNetCore.Authorization;

namespace AdminAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    [ApiController]
    public class MovieController : ControllerBase
    {
        MovieService _movieService;

        public MovieController(MovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<Movie>> Get(long id)
        {
            var movie = await _movieService.GetByIdAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetAllAsync()
        {
            var movies = await _movieService.GetAllAsync();
            if (movies != null)
            {
                return Ok(movies);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Post(Movie movie)
        {
            if (movie == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _movieService.AddMovieAsync(movie);

            return CreatedAtAction(nameof(Get), new { id = movie.Id }, movie);
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult> Delete(long id)
        {
            var movie = await _movieService.GetByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            await _movieService.DeleteMovieAsync(movie);

            return Ok();
        }
    }
}