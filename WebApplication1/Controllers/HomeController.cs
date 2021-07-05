using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFeatureManagerSnapshot _featureManagerSnapshot;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IFeatureManagerSnapshot featureManagerSnapshot)
        {
            _logger = logger;
            _featureManagerSnapshot = featureManagerSnapshot;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogDebug("Invoking IsEnabledAsync");
            var isArtIntegrationEnabled = await _featureManagerSnapshot.IsEnabledAsync("ArtIntegration");
            _logger.LogDebug($"isArtIntegrationEnabled: {isArtIntegrationEnabled}");
            var areTrackElementsEnabled = await _featureManagerSnapshot.IsEnabledAsync("TrackElements");
            _logger.LogDebug($"areTrackElementsEnabled: {areTrackElementsEnabled}");
            return View();
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
}