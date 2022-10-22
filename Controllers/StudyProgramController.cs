using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMUniGraduation.Data;
using MMUniGraduation.Models;
using MMUniGraduation.Models.Create;
using MMUniGraduation.Services.Interfaces;
using System.Threading.Tasks;

namespace MMUniGraduation.Controllers
{
    public class StudyProgramController : Controller
    {
        private readonly IStudyProgramService _studyProgramService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public StudyProgramController(IStudyProgramService studyProgramService, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _studyProgramService = studyProgramService;
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index1()
        {
            return View();
        }

        // GET: StudyPrograms
        public async Task<IActionResult> Index()
        {
            return View(await _context.StudyPrograms.ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateStudyProgram model)
        {
            this.TempData["Message"] = "Try to create program.";

            var user = await _userManager.GetUserAsync(this.User);
            
            //await _studyProgramService.CreateAsync(input, user.Id);
            await _studyProgramService.CreateAsync(model);

            this.TempData["Message"] = "Program created successfully!";

            return RedirectToAction("Index","Home");
            //return Content($" Name: {Name}, Desc: {Description}");
            //return View();
        }
    }
}
