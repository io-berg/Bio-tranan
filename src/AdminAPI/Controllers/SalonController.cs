using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTheaterCore.Models;
using MovieTheaterCore.Services;

namespace AdminAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class SalonController : ControllerBase
    {
        SalonService _salonService;

        public SalonController(SalonService salonService)
        {
            _salonService = salonService;
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<Salon>> Get(long id)
        {
            var salon = await _salonService.GetByIdAsync(id);

            if (salon == null)
            {
                return NotFound();
            }

            return Ok(salon);
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Salon>>> GetAllAsync()
        {
            var salons = await _salonService.GetAllAsync();
            if (salons != null)
            {
                return Ok(salons);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Post(Salon salon)
        {
            await _salonService.AddSalonAsync(salon);

            return CreatedAtAction(nameof(Get), new { id = salon.Id }, salon);
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult> Delete(long id)
        {
            var salon = await _salonService.GetByIdAsync(id);
            if (salon == null)
            {
                return NotFound();
            }

            await _salonService.DeleteSalonAsync(salon);

            return Ok();
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult> Put(long id, Salon salon)
        {
            await _salonService.UpdateSalonAsync(id, salon);

            return NoContent();
        }
    }
}