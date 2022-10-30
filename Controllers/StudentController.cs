using Microsoft.AspNetCore.Mvc;
using MMUniGraduation.Data;
using MMUniGraduation.Models;
using System.Linq;
using MMUniGraduation.Services.Interfaces;

namespace MMUniGraduation.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICourseService _courseService;
        public StudentController(ApplicationDbContext context, ICourseService courseService)
        {
            _context = context;
            _courseService = courseService;
        }
        
        public IActionResult Index()
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

            var viewModel = new Student
            {
                CurrentCourse = _context.Courses.FirstOrDefault(c => c.Id == 2)
            };

            return View();
        }
        public IActionResult Edit()
        {
            return View();
        }

    }
}
