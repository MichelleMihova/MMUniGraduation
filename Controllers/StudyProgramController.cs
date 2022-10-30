using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MMUniGraduation.Models;
using MMUniGraduation.Models.Create;
using MMUniGraduation.Services.Interfaces;
using MMUniGraduation.ViewModels;
using System.Threading.Tasks;

namespace MMUniGraduation.Controllers
{
    public class StudyProgramController : Controller
    {
        private readonly IStudyProgramService _studyProgramService;
        private readonly UserManager<ApplicationUser> _userManager;
        public StudyProgramController(IStudyProgramService studyProgramService, UserManager<ApplicationUser> userManager)
        {
            _studyProgramService = studyProgramService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var viewModel = new IndexViewModel()
            {
                StudyPrograms = _studyProgramService.GetAllPrograms<IndexStudyProgramViewModel>()
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateStudyProgram model)
        {
            var user = await _userManager.GetUserAsync(this.User);

            await _studyProgramService.CreateAsync(model/*,user.Id*/);

            this.TempData["Message"] = "Program created successfully!";

            return RedirectToAction("Index", "Home");
        }
    }
}
