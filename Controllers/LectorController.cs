using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MMUniGraduation.Data;
using MMUniGraduation.Models;
using MMUniGraduation.Services.Interfaces;
using MMUniGraduation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMUniGraduation.Controllers
{
    public class LectorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICourseService _courseService;
        private readonly IStudyProgramService _studyProgramService;
        private readonly ILectureService _lectureService;
        private readonly ILectorService _lectorService;
        private readonly UserManager<ApplicationUser> _userManager;

        public LectorController(ApplicationDbContext context, ICourseService courseService,
            IStudyProgramService studyProgramService, ILectureService lectureService, ILectorService lectorService,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _courseService = courseService;
            _studyProgramService = studyProgramService;
            _lectureService = lectureService;
            _lectorService = lectorService;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(this.User);
            var lector = _context.Lectors.FirstOrDefault(x => x.UserId == user.Id);

            var courses = _context.Courses.Where(x => x.CreatorId == lector.UserId);
            var programCourses = new Dictionary<string, List<Course>>();

            if (courses.Any())
            {
                foreach (var item in courses)
                {
                    if (!programCourses.Keys.Contains(item.Signature))
                    {
                        var coursesForProgram = _context.Courses.Where(x => x.CreatorId == lector.UserId && x.Signature == item.Signature).OrderBy(x => x.ParetntId).ToList();
                        programCourses.Add(item.Signature, coursesForProgram);
                    }
                }
            }

            var photo = _context.Images.Where(x => x.LectorId == lector.Id).Select(x => x.Id + '.' + x.Extension).FirstOrDefault();

            var viewModel = new IndexLectorViewModel
            {
                FirstName = lector.FirstName,
                LastName = lector.LastName,
                Bio = lector.Bio,
                PhoneNumber = lector.PhoneNumber,
                Image = photo,
                Courses = courses,
                ProgramCourses = programCourses
            };

            return View(viewModel);
        }

        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(this.User);
            var lector = _context.Lectors.FirstOrDefault(x => x.UserId == user.Id);

            var viewModel = new EditLectorViewModel
            {
                Id = lector.Id,
                FirstName = lector.FirstName,
                LastName = lector.LastName,
                Bio = lector.Bio
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditLectorViewModel input)
        {
            await _lectorService.EditInfo(input);

            return RedirectToAction("Index", "Lector");
        }

        public async Task<IActionResult> EditCourses()
        {
            var user = await _userManager.GetUserAsync(this.User);

            var viewModel = new AllCoursesViewModel
            {
                AllCourses = _context.Courses.Where(x => x.CreatorId == user.Id).ToList(),
                AllPrograms = _context.StudyPrograms.ToList()
            };

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult RelatedCourses(int programId)
        {
            var courses = _courseService.GetAllAsKeyValuePairs(programId);
            IEnumerable<SelectListItem> dropdownData = courses.Select(item => new SelectListItem { Value = item.Key, Text = item.Value }).ToList(); ;

            return Json(new SelectList(dropdownData, "Value", "Text"));
        }

        [HttpGet]
        public ActionResult RelatedLectures(int courseId)
        {
            var lectures = _lectureService.GetAllAsKeyValuePairs(courseId);
            IEnumerable<SelectListItem> dropdownData = lectures.Select(item => new SelectListItem { Value = item.Key, Text = item.Value }).ToList(); ;

            return Json(new SelectList(dropdownData, "Value", "Text"));
        }

        public IActionResult Assessment()
        {
            var viewModel = new AssessmentsViewModel
            {
                AllCourses = _context.Courses.ToList(),
                StudyPrograms = _studyProgramService.GetAllAsKeyValuePairs(),
                Courses = _courseService.GetAllAsKeyValuePairs(),
                Lectures = _lectureService.GetAllAsKeyValuePairs()
            };

            var lectures = new List<Lecture>();
            var homeworks = new List<Homework>();
            var students = new List<Student>();
            var skippingAssessments = new List<SkippingAssignment>();

            foreach (var course in viewModel.AllCourses)
            {
                lectures = _context.Lectures.Where(x => x.CourseId == course.Id).ToList();

                course.Lectures = lectures;

                skippingAssessments = _context.SkippingAssignments.Where(x => x.CourseId == course.Id && x.Grade == 0).ToList();
                course.SkippingAssignments = skippingAssessments;

                var studentsId = _context.SkippingAssignments.Select(x => x.StudentId).ToList();
                foreach (var id in studentsId)
                {
                    var student = _context.Students.FirstOrDefault(x => x.UserId == id);
                    if (!students.Contains(student))
                    {
                        students.Add(student);
                    }
                }

                foreach (var lecture in course.Lectures)
                {
                    homeworks = _context.Homeworks.Where(x => x.LectureId == lecture.Id && x.Grade == 0).ToList();
                    lecture.Homeworks = homeworks;

                    studentsId = _context.Homeworks.Select(x => x.StudentId).ToList();
                    foreach (var id in studentsId)
                    {
                        var student = _context.Students.FirstOrDefault(x => x.UserId == id);
                        if (!students.Contains(student))
                        {
                            students.Add(student);
                        }
                    }
                }
            }

            viewModel.Students = students;

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Assessment(AssessmentsViewModel input)
        {
            var viewModel = new AssessmentsViewModel
            {
                AllCourses = _context.Courses.ToList(),
                StudyPrograms = _studyProgramService.GetAllAsKeyValuePairs(),
                Courses = _courseService.GetAllAsKeyValuePairs(),
                Lectures = _lectureService.GetAllAsKeyValuePairs()
            };

            var lectures = new List<Lecture>();
            var homeworks = new List<Homework>();
            var students = new List<Student>();
            var skippingAssesments = new List<SkippingAssignment>();

            List<Course> courses = null;
            if (input.CourseId != 0 || input.ProgramId != 0)
            {
                if (input.CourseId != 0)
                {
                    courses = _context.Courses.Where(x => x.Id == input.CourseId).ToList();
                }
                else if (input.ProgramId != 0)
                {
                    courses = _context.Courses.Where(x => x.StudyProgramId == input.ProgramId).ToList();
                }
                else
                {
                    courses = _context.Courses.ToList();
                }
            }
            else
            {
                courses = _context.Courses.ToList();
            }

            if (input.LectureId != 0)
            {
                var lecture = _context.Lectures.FirstOrDefault(l => l.Id == input.LectureId);

                if (input.KindOfAssessment.ToUpper() == "HOMEWORKS")
                {
                    homeworks = _context.Homeworks.Where(l => l.LectureId == lecture.Id && l.Grade == 0 && l.HomeworkTitle.ToUpper() != "EXAM").ToList();
                }
                else if (input.KindOfAssessment.ToUpper() == "EXAMSOLUTIONS")
                {
                    homeworks = _context.Homeworks.Where(l => l.LectureId == lecture.Id && l.Grade == 0 && l.HomeworkTitle.ToUpper() == "EXAM").ToList();
                }
                else if (input.KindOfAssessment.ToUpper() == "ALLASSESSMENTS")
                {
                    homeworks = _context.Homeworks.Where(l => l.LectureId == lecture.Id && l.Grade == 0).ToList();
                }

                lecture.Homeworks = homeworks;

                var studentsId = _context.Homeworks.Select(x => x.StudentId).ToList();
                foreach (var id in studentsId)
                {
                    var student = _context.Students.FirstOrDefault(x => x.UserId == id);
                    if (!students.Contains(student))
                    {
                        students.Add(student);
                    }
                }
            }
            else
            {
                foreach (var course in courses)
                {
                    if (input.KindOfAssessment.ToUpper() == "SKIPPINGEXAMSOLUTIONS" || input.KindOfAssessment.ToUpper() == "ALLASSESSMENTS")
                    {
                        skippingAssesments = _context.SkippingAssignments.Where(x => x.CourseId == course.Id && x.Grade == 0).ToList();

                        course.SkippingAssignments = skippingAssesments;
                        var studentsId = _context.SkippingAssignments.Select(x => x.StudentId).ToList();

                        foreach (var id in studentsId)
                        {
                            var student = _context.Students.FirstOrDefault(x => x.UserId == id);
                            if (!students.Contains(student))
                            {
                                students.Add(student);
                            }
                        }

                        if (input.KindOfAssessment.ToUpper() == "ALLASSESSMENTS")
                        {
                            lectures = _context.Lectures.Where(l => l.CourseId == course.Id).ToList();
                            foreach (var lecture in course.Lectures)
                            {
                                homeworks = _context.Homeworks.Where(l => l.LectureId == lecture.Id && l.Grade == 0).ToList();
                                lecture.Homeworks = homeworks;
                            }

                            var studentsHWId = _context.Homeworks.Select(x => x.StudentId).ToList();
                            foreach (var id in studentsHWId)
                            {
                                var student = _context.Students.FirstOrDefault(x => x.UserId == id);
                                if (!students.Contains(student))
                                {
                                    students.Add(student);
                                }
                            }
                        }
                    }
                    else
                    {
                        lectures = _context.Lectures.Where(l => l.CourseId == course.Id).ToList();
                        course.Lectures = lectures;

                        foreach (var lecture in course.Lectures)
                        {
                            if (input.KindOfAssessment.ToUpper() == "HOMEWORKS")
                            {
                                homeworks = _context.Homeworks.Where(l => l.LectureId == lecture.Id && l.Grade == 0 && l.HomeworkTitle.ToUpper() != "EXAM").ToList();
                            }
                            else if (input.KindOfAssessment.ToUpper() == "EXAMSOLUTIONS")
                            {
                                homeworks = _context.Homeworks.Where(l => l.LectureId == lecture.Id && l.Grade == 0 && l.HomeworkTitle.ToUpper() == "EXAM").ToList();
                            }
                            else if (input.KindOfAssessment.ToUpper() == "ALLASSESSMENTS")
                            {
                                homeworks = _context.Homeworks.Where(l => l.LectureId == lecture.Id && l.Grade == 0).ToList();
                            }

                            lecture.Homeworks = homeworks;

                            var studentsId = _context.Homeworks.Select(x => x.StudentId).ToList();
                            foreach (var id in studentsId)
                            {
                                var student = _context.Students.FirstOrDefault(x => x.UserId == id);
                                if (!students.Contains(student))
                                {
                                    students.Add(student);
                                }
                            }
                        }
                    }
                }
            }

            viewModel.Students = students;

            return View(viewModel);
        }
    }
}
