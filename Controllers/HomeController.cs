using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MMUniGraduation.Models;
using MMUniGraduation.Services.Interfaces;
using MMUniGraduation.ViewModels;
using System.Diagnostics;
using ErrorViewModel = MMUniGraduation.Models.ErrorViewModel;

namespace MMUniGraduation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStudyProgramService _studyProgramService;

        public HomeController(ILogger<HomeController> logger, IStudyProgramService studyProgramService)
        {
            _logger = logger;
            _studyProgramService = studyProgramService;
        }

        public IActionResult Index()
        {
            var viewModel = new IndexViewModel()
            {
                StudyPrograms = _studyProgramService.GetPrograms<IndexStudyProgramViewModel>(3)
            };

            return View(viewModel);
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
