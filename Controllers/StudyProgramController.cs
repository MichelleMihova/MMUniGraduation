using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MMUniGraduation.Data;
using MMUniGraduation.Models;
using MMUniGraduation.Models.Create;
using MMUniGraduation.Services.Interfaces;
using MMUniGraduation.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace MMUniGraduation.Controllers
{
    public class StudyProgramController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStudyProgramService _studyProgramService;
        private readonly UserManager<ApplicationUser> _userManager;
        public StudyProgramController(ApplicationDbContext context, IStudyProgramService studyProgramService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
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
            var programNames = _context.StudyPrograms.Select(x => x.Name);

            if (programNames.Contains(model.Name))
            {
                this.TempData["Message"] = "The program has not been created yet! There is an existing program with the same name!";
                return View(model);
            }

            await _studyProgramService.CreateAsync(model);

            this.TempData["Message"] = "Program created successfully!";

            return RedirectToAction("Index", "Home");
        }
    }
}
