using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private MovieViewingViewModelService _movieViewingViewModelService;

    public HomeController(ILogger<HomeController> logger, MovieViewingViewModelService movieViewingViewModelService)
    {
        _logger = logger;
        _movieViewingViewModelService = movieViewingViewModelService;
    }

    public async Task<IActionResult> IndexAsync()
    {
        HomeViewModel model = new HomeViewModel();
        model.Viewings = await _movieViewingViewModelService.GetUpcomingViewingModelsAsync();

        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
