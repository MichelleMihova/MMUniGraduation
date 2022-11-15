using Microsoft.AspNetCore.Mvc;
using MMUniGraduation.Data;
using MMUniGraduation.Models;
using System.Linq;
using MMUniGraduation.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MMUniGraduation.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICourseService _courseService;
        private readonly UserManager<ApplicationUser> _userManager;
        public StudentController(ApplicationDbContext context, ICourseService courseService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _courseService = courseService;
            _userManager = userManager;
        }

        //public IActionResult Index()
        public async Task<IActionResult> Index()
        {
            //display image
            //var content = db.Contents.Select(s => new
            //{
            //    s.ID,
            //    s.Title,
            //    s.Image,
            //    s.Contents,
            //    s.Description
            //});
            //List<ContentViewModel> contentModel = content.Select(item => new ContentViewModel()
            //{
            //    ID = item.ID,
            //    Title = item.Title,
            //    Image = item.Image,
            //    Description = item.Description,
            //    Contents = item.Contents
            //}).ToList();
            //return View(contentModel);

            var user = await _userManager.GetUserAsync(this.User);
            var student = _context.Students.FirstOrDefault(x => x.UserId == user.Id);

            var viewModel = new Student
            {
                CurrentCourse = _context.Courses.FirstOrDefault(c => c.Id == student.CurrentCourseId),
                PassedCourses = await _context.Courses.Where(c => c.StudyProgramId == 1).ToListAsync()
            };

            return View(viewModel);
        }
        public IActionResult Edit()
        {
            return View();
        }

    }
}
