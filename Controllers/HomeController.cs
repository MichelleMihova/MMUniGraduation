using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MMUniGraduation.Data;
using MMUniGraduation.Models;
using MMUniGraduation.Services.Interfaces;
using MMUniGraduation.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ErrorViewModel = MMUniGraduation.Models.ErrorViewModel;

namespace MMUniGraduation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IStudyProgramService _studyProgramService;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IStudyProgramService studyProgramService)
        {
            _logger = logger;
            _context = context;
            _studyProgramService = studyProgramService;
        }

        public IActionResult Index1()
        {
            return View();
        }

        public IActionResult Index()
        {
            var viewModel = new IndexViewModel()
            {
                StudyPrograms = _studyProgramService.GetPrograms<IndexStudyProgramViewModel>(3)
            };

            return View(viewModel);
            //return View(await _context.StudyPrograms.ToListAsync());
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
