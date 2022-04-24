using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using MovieTheaterCore.Models;
using Microsoft.EntityFrameworkCore;
using MovieTheaterCore.Services;
using Microsoft.AspNetCore.Authorization;

namespace AdminAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class MovieViewingController : ControllerBase
    {
        MovieViewingService _movieViewingService;

        public MovieViewingController(MovieViewingService movieViewingService)
        {
            _movieViewingService = movieViewingService;
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<MovieViewing>> Get(long id)
        {
            var movieViewing = await _movieViewingService.GetByIdAsync(id);

            if (movieViewing == null)
            {
                return NotFound();
            }

            return Ok(movieViewing);
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<MovieViewing>>> GetAllAsync()
        {
            if (await _movieViewingService.GetAllAsync() != null)
            {
                return Ok(await _movieViewingService.GetAllAsync());
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Post(MovieViewingCreationModel movieViewing)
        {
            MovieViewing addedViewing;

            try
            {
                addedViewing = await _movieViewingService.AddMovieViewingAsync(movieViewing);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return CreatedAtAction(nameof(Get), new { id = addedViewing.Id }, addedViewing);
        }
    }
}