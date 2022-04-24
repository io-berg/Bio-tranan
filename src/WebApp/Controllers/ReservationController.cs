using Microsoft.AspNetCore.Mvc;
using MovieTheaterCore.Models;
using MovieTheaterCore.Services;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class ReservationController : Controller
    {
        ReservationService _reservationService;
        ReservationViewModelService _reservationViewModelService;

        public ReservationController(ReservationService reservationService, ReservationViewModelService reservationViewModelService)
        {
            _reservationService = reservationService;
            _reservationViewModelService = reservationViewModelService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Info(string reservationCode)
        {
            var model = await _reservationViewModelService.GetModelByCode(reservationCode);
            if (model == null) return NotFound();

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteReservation(string reservationCode)
        {
            var reservation = await _reservationService.GetByCode(reservationCode);
            if (reservation == null) return NotFound();
            await _reservationService.DeleteReservationAsync(reservation);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> PostReservation(ReservationCreationModel reservation)
        {
            Reservation createdReservation;
            try
            {
                createdReservation = await _reservationService.AddReservationAsync(reservation);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            if (createdReservation == null) return BadRequest("Reservation could not be created");

            return RedirectToAction("Thanks", "Reservation", new { code = createdReservation.ReservationCode });
        }

        [HttpGet]
        public async Task<IActionResult> Thanks(string code)
        {
            var model = await _reservationViewModelService.GetModelByCode(code);

            if (model == null) return NotFound();

            return View(model);
        }
    }
}