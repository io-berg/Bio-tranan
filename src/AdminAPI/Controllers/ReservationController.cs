using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTheaterCore.Models;
using MovieTheaterCore.Services;

namespace AdminAPI.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ReservationController : ControllerBase
    {
        ReservationService _reservationService;

        public ReservationController(ReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<Reservation>> Get(long id)
        {
            var reservation = await _reservationService.GetByIdAsync(id);

            if (reservation == null)
            {
                return NotFound();
            }

            return Ok(reservation);
        }

        [HttpGet("{date:datetime}")]
        public async Task<ActionResult<IEnumerable<Reservation>>> Get(DateTime date)
        {
            var reservations = await _reservationService.GetByDate(date);

            if (reservations == null)
            {
                return NotFound();
            }

            return reservations;
        }

        [HttpGet("{viewingId:long}")]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetByViewingId(long viewingId)
        {
            var reservations = await _reservationService.GetByViewingId(viewingId);

            if (reservations == null) return NotFound("No viewing found for this id");
            if (reservations.Count() == 0) return NoContent();

            return Ok(reservations);
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Reservation>>> Get()
        {
            var reservations = await _reservationService.GetAllAsync();
            if (reservations != null)
            {
                return Ok(reservations);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Post(ReservationCreationModel reservation)
        {
            Reservation insertedReservation;

            try
            {
                insertedReservation = await _reservationService.AddReservationAsync(reservation);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return CreatedAtAction(nameof(Get), new { id = insertedReservation.Id }, insertedReservation);
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult> Delete(long id)
        {
            var reservation = await _reservationService.GetByIdAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            await _reservationService.DeleteReservationAsync(reservation);

            return NoContent();
        }
    }
}