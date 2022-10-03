using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMUniGraduation.Data;
using MMUniGraduation.Services.Interfaces;
using System.Threading.Tasks;

namespace MMUniGraduation.Controllers
{
    public class StudyProgramController : Controller
    {
        // private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStudyProgramService _studyProgramService;
        private readonly ApplicationDbContext _context;
        public StudyProgramController(IStudyProgramService studyProgramService, ApplicationDbContext context)
        {
            _studyProgramService = studyProgramService;
            _context = context;
        }
        public IActionResult Index1()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string Name, string Description)
        {
            this.TempData["Message"] = "Try to create program.";

            //var user = await _userManager.GetUserAsync(this.User);
            //await _studyProgramService.CreateAsync(input, user.Id);
            await _studyProgramService.CreateAsync(Name, Description);

            this.TempData["Message"] = "Program created successfully!";

            return RedirectToAction("Index","Home");
            //return Content($" Name: {Name}, Desc: {Description}");
            //return View();
        }

        // GET: StudyPrograms
        public async Task<IActionResult> Index()
        {
            return View(await _context.StudyPrograms.ToListAsync());
        }
    }
}
