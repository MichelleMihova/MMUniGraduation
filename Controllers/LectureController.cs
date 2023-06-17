using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MMUniGraduation.Data;
using MMUniGraduation.Models;
using MMUniGraduation.Models.Create;
using MMUniGraduation.Services.Interfaces;
using MMUniGraduation.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMUniGraduation.Controllers
{
    public class LectureController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILectureService _lectureService;
        private readonly ICourseService _courseService;
        private readonly UserManager<ApplicationUser> _userManager;
        public LectureController(ApplicationDbContext context, ILectureService lectureService, ICourseService courseService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _lectureService = lectureService;
            _courseService = courseService;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin, Teacher")]
        public IActionResult Create()
        {
            var viewModel = new CreateLecture
            {
                Courses = _courseService.GetAllAsKeyValuePairs(),
                AllLectures = _lectureService.GetAllAsKeyValuePairs()
            };

            return this.View(viewModel);
        }

        [HttpGet]
        public ActionResult GetLectures(int courseId)
        {
            var lectures = _lectureService.GetAllAsKeyValuePairs(courseId);

            IEnumerable<SelectListItem> dropdownData = lectures.Select(item => new SelectListItem { Value = item.Key, Text = item.Value }).ToList(); ;

            return Json(new SelectList(dropdownData, "Value", "Text"));
        }
        [Authorize(Roles = "Admin, Teacher")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateLecture input)
        {
            if (!ModelState.IsValid)
            {
                input.Courses = _courseService.GetAllAsKeyValuePairs();
                input.AllLectures = _lectureService.GetAllAsKeyValuePairs();
            }
            var user = await _userManager.GetUserAsync(this.User);
            var lectureNames = _context.Lectures.Where(x => x.CourseId == input.CourseId).Select(x => x.Name);

            if (lectureNames.Contains(input.Name))
            {
                this.TempData["Message"] = "The lecture has not been created yet! There is an existing lecture with the same name!";
                return View(input);
            }
            await _lectureService.CreateLectureAsync(input, user);

            this.TempData["Message"] = "Lecture created successfully!";

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> AddExamSolution(IFormFile file, int lectureId)
        {
            var user = await _userManager.GetUserAsync(this.User);
            var currLecture = await _context.Lectures.FirstOrDefaultAsync(x => x.Id == lectureId);

            await _lectureService.AddExamSolutionToLecture(lectureId, file, user.Id);

            this.TempData["Message"] = "Exam solution added successfully!";


            //return RedirectToAction("Index", "Home");
            return RedirectToAction("Index", "Course", new { courseId = currLecture.CourseId });
        }
        
        public async Task<IActionResult> AddHomework(IFormFile file, int lectureId)
        {
            var user = await _userManager.GetUserAsync(this.User);
            var currLecture = await _context.Lectures.FirstOrDefaultAsync(x => x.Id == lectureId);

            await _lectureService.AddHomeworkToLecture(lectureId, file, user.Id);

            this.TempData["Message"] = "Homework added successfully!";

            return RedirectToAction("Index", "Course", new { courseId = currLecture.CourseId });
        }

        public async Task<IActionResult> EditHomework(AssessmentsViewModel input)
        {
            await _lectureService.EditHomework(input.HomeworkId, input.HomeworkGrade, input.HomeworkComment);

            var hw = await _context.Homeworks.FirstOrDefaultAsync(x => x.Id == input.HomeworkId);
            var student = await _context.Students.FirstOrDefaultAsync(x => x.UserId == hw.StudentId);
            var lecture = await _context.Lectures.FirstOrDefaultAsync(x => x.Id == hw.LectureId);

            if (hw.HomeworkTitle.ToUpper() == "EXAM" && hw.Grade >= lecture.RequiredGrade)
            {
                //TO DO.. change StudentId = student.UserId
                //move curr course as passed 
                //var passedCourse = new StudentCourses
                //{
                //    StudentId = student.Id,
                //    CourseId = input.CourseId,
                //    FinalGrade = input.HomeworkGrade,
                //    IsPassed = true
                //};

                var studentCourse = _context.StudentCourses.FirstOrDefault(x => x.CourseId == lecture.CourseId && !x.IsPassed && x.StudentId == student.Id);
                studentCourse.IsPassed = true;
                studentCourse.FinalGrade = input.HomeworkGrade;
                //await _context.StudentCourses.AddAsync(passedCourse);
                //await _context.SaveChangesAsync();

                //student.CurrentCourseId = null;

                _context.SaveChanges();
            }

            return RedirectToAction("Assessment", "Lector");
        }

        public async Task<IActionResult> EditSkippingAssignment(AssessmentsViewModel input)
        {
            await _lectureService.EditSkippingAssignment(input.HomeworkId, input.HomeworkGrade, input.HomeworkComment);

            //var hw = await _context.Homeworks.FirstOrDefaultAsync(x => x.Id == input.HomeworkId);
            //var student = await _context.Students.FirstOrDefaultAsync(x => x.UserId == hw.StudentId);
            //var lecture = await _context.Lectures.FirstOrDefaultAsync(x => x.Id == hw.LectureId);

            return RedirectToAction("Assessment", "Lector");
        }
        public async Task<IActionResult> EditLecture(EditCourseViewModel input)
        {
            await _lectureService.EditLecture(input);
            return RedirectToAction("Edit", "Course", new { courseId = input.CourseId });
        }

        public async Task<IActionResult> DeleteLectureMaterial(string lectureFileId, int courseId)
        {
            await _lectureService.DeleteLectureMaterial(lectureFileId);
            return RedirectToAction("Edit", "Course", new { courseId = courseId });
        }
        
        public async Task<IActionResult> DeleteHomework(int lectureId, int courseId, string name)
        {
            await _lectureService.DeleteHomework(lectureId);
            return RedirectToAction(name, "Course", new { courseId = courseId });
        }

        public async Task<IActionResult> DeleteLecture(int lectureId, int courseId)
        {
            await _lectureService.DeleteLectureMaterial(lectureId);
            await _lectureService.DeleteHomework(lectureId);
            await _lectureService.DeleteLecture(lectureId);

            return RedirectToAction("Edit", "Course", new { courseId = courseId });
        }

        public async Task<IActionResult> SetConstraints(EditCourseViewModel input)
        {
            //if homeworkGrade for currStudentId
            await _lectureService.EditLectureFile(input);

            return RedirectToAction("Edit", "Course", new { courseId = input.CourseId });
        }
            
    }
}
