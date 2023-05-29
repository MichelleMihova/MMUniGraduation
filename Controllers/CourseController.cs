using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMUniGraduation.Data;
using MMUniGraduation.Models;
using MMUniGraduation.Models.Create;
using MMUniGraduation.Services.Interfaces;
using MMUniGraduation.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MMUniGraduation.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICourseService _courseService;
        private readonly IStudyProgramService _studyProgramService;
        private readonly ILectureService _lectureService;
        private readonly IWebHostEnvironment _webHost;
        private readonly UserManager<ApplicationUser> _userManager;

        public CourseController(ICourseService courseService, IStudyProgramService studyProgramService,
            ILectureService lectureService, ApplicationDbContext context, IWebHostEnvironment webHost,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _courseService = courseService;
            _studyProgramService = studyProgramService;
            _lectureService = lectureService;
            _webHost = webHost;
            _userManager = userManager;
        }

        public FileResult DownloadFile(string fileName, string type)
        {
            string path;
            if (type == "homework")
            {
                path = Path.Combine(_webHost.WebRootPath, "homeworks/") + fileName;
            }
            else if (type == "examSolution")
            {
                path = Path.Combine(_webHost.WebRootPath, "examSolutions/") + fileName;
            }
            else if (type == "skippingExamSolution")
            {
                path = Path.Combine(_webHost.WebRootPath, "skippingExamSolutions/") + fileName;
            }
            else
            {
                path = Path.Combine(_webHost.WebRootPath, "files/") + fileName;
            }

            byte[] bytes = System.IO.File.ReadAllBytes(path);

            return File(bytes, "application/octet-stream", fileName);
        }

        public async Task<IActionResult> Index(int courseId, bool goToExam)
        {
            var user = await _userManager.GetUserAsync(this.User);
            var student = _context.Students.FirstOrDefault(x => x.UserId == user.Id);

            var currentCourse = _context.Courses.FirstOrDefault(x => x.Id == courseId);
            currentCourse.Lectures = _context.Lectures.Where(l => l.CourseId == courseId).ToList();

            var studentCurrentCourse = new StudentCourses();

            DateTime? EndDateTimeForSkipExam = null;

            var textMaterial = new List<LectureFile>();
            var homeworkMaterial = new List<LectureFile>();
            var skippingMaterial = new List<LectureFile>();
            var skippingAssignments = new List<SkippingAssignment>();

            foreach (var lecture in currentCourse.Lectures)
            {
                if (student != null)
                {
                    studentCurrentCourse = _context.StudentCourses.FirstOrDefault(x => x.StudentId == student.Id && x.CourseId == currentCourse.Id);

                    if (studentCurrentCourse != null && studentCurrentCourse.EndDateTimeForSkipping != null)
                    {
                        EndDateTimeForSkipExam = studentCurrentCourse.EndDateTimeForSkipping;
                    }

                    var hw = _context.Homeworks.Where(l => l.LectureId == lecture.Id && l.StudentId == student.UserId).ToList();
                    decimal avarageHWgrade = 0;
                    int cnt = 0;
                    if (hw.Any())
                    {
                        foreach (var item in hw)
                        {
                            avarageHWgrade += item.Grade;
                            cnt++;
                        }
                        avarageHWgrade /= cnt;
                    }

                    if (goToExam || studentCurrentCourse.EndDateTimeForSkipping != null)
                    {
                        if (studentCurrentCourse.EndDateTimeForSkipping == null)
                        {
                            studentCurrentCourse.EndDateTimeForSkipping = System.DateTime.Now.AddHours(3);
                            _context.SaveChanges();
                        }
                        skippingMaterial = _context.LectureFiles.Where(x => x.LectureId == null && x.CourseId == currentCourse.Id && x.FileTitle == "SKIPPINGEXAM").ToList();
                        currentCourse.SkippingCourseMaterials = skippingMaterial;

                        skippingAssignments = _context.SkippingAssignments.Where(x => x.CourseId == currentCourse.Id && x.StudentId == student.UserId).ToList();
                        currentCourse.SkippingAssignments = skippingAssignments;

                    }

                    textMaterial = _context.LectureFiles.Where(l => l.LectureId == lecture.Id && l.MinHWGrade <= avarageHWgrade && l.DateTimeToShow <= System.DateTime.Now && l.FileTitle == "LECTURE").ToList();
                    homeworkMaterial = _context.LectureFiles.Where(l => l.LectureId == lecture.Id && l.FileTitle == "HOMEWORK").ToList();
                }
                else
                {
                    textMaterial = _context.LectureFiles.Where(l => l.LectureId == lecture.Id && l.FileTitle == "LECTURE").ToList();
                    homeworkMaterial = _context.LectureFiles.Where(l => l.LectureId == lecture.Id && l.FileTitle == "HOMEWORK").ToList();

                    skippingMaterial = _context.LectureFiles.Where(x => x.LectureId == null && x.CourseId == currentCourse.Id && x.FileTitle == "SKIPPINGEXAM").ToList();
                    currentCourse.SkippingCourseMaterials = skippingMaterial;
                }

                lecture.TextMaterials = textMaterial;
            }

            var homework = new List<Homework>();
            foreach (var lecture in currentCourse.Lectures)
            {
                if (student != null)
                {
                    homework = _context.Homeworks.Where(l => l.LectureId == lecture.Id && l.StudentId == student.UserId).ToList();
                    lecture.Homeworks = homework;
                }
            }

            var viewModel = new IndexCourseViewModel
            {
                Course = currentCourse,
                EndDateTime = EndDateTimeForSkipExam,
                SkipCourse = goToExam,
                Student = student,
                HWMaterials = homeworkMaterial,
                StudentCourse = studentCurrentCourse
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Admin, Teacher")]
        public IActionResult Create()
        {
            var viewModel = new CreateCourse
            {
                StudyPrograms = _studyProgramService.GetAllAsKeyValuePairs(),
                Courses = _courseService.GetAllAsKeyValuePairs()
            };

            return this.View(viewModel);
        }

        [Authorize(Roles = "Admin, Teacher")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateCourse input)
        {
            if (!ModelState.IsValid)
            {
                input.StudyPrograms = _studyProgramService.GetAllAsKeyValuePairs();

                return this.View(input);
            }

            var user = await _userManager.GetUserAsync(this.User);

            await _courseService.CreateCourseAsync(input, user);

            this.TempData["Message"] = "Course created successfully!";

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> AllCourses(int studyProgramId, string message)
        {
            var user = await _userManager.GetUserAsync(this.User);
            var student = _context.Students.FirstOrDefault(x => x.UserId == user.Id);

            var currCourse = new Course();
            var passedCourses = new List<Course>();
            //var currentCourses = new List<Course>();
            if (student != null)
            {
                //get current courseId from selected program for logged user
                var currentCoursesId = _context.StudentCourses.FirstOrDefault(x => x.StudentId == student.Id && x.IsPassed == false && x.ProgramId == studyProgramId);

                if (currentCoursesId != null)
                {
                    currCourse = _context.Courses.FirstOrDefault(x => x.Id == currentCoursesId.CourseId);
                }

                //get all passed courseId's from selected program for logged user
                var passedCoursesId = _context.StudentCourses.Where(x => x.StudentId == student.Id && x.IsPassed == true && x.ProgramId == studyProgramId).Select(x => x.CourseId);

                if (passedCoursesId.Any())
                {
                    foreach (var item in passedCoursesId)
                    {
                        var course = _context.Courses.FirstOrDefault(x => x.Id == item);
                        passedCourses.Add(course);
                    }
                }
            }

            var viewModel = new AllCoursesViewModel
            {
                //NextCourseName = _courseService.GetNextCourseSuggestion(student),
                NextCourseName = _courseService.GetNextCourseSuggestion(student, studyProgramId),
                AllCourses = await _context.Courses.Where(c => c.StudyProgramId == studyProgramId).ToListAsync(),
                Signature = _context.StudyPrograms.FirstOrDefault(x => x.Id == studyProgramId).Name,
                PassedCourses = passedCourses,
                //CurrentCourses = currentCourses,
                CurrentUserCourse = currCourse
            };

            if (message != null)
            {
                this.TempData["Message"] = message;
            }

            return View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> AssignUserToCourse(int courseId)
        {
            var user = await _userManager.GetUserAsync(this.User);

            var student = _context.Students.FirstOrDefault(x => x.UserId == user.Id);
            var course = _context.Courses.FirstOrDefault(x => x.Id == courseId);

            if (student != null)
            {
                var passedCoursesId = _context.StudentCourses.Where(x => x.StudentId == student.Id && x.IsPassed == true && x.ProgramId == course.StudyProgramId).Select(x => x.CourseId);
                var passedCourses = new List<Course>();

                if (passedCoursesId.Any())
                {
                    foreach (var id in passedCoursesId)
                    {
                        var c = _context.Courses.FirstOrDefault(x => x.Id == id);
                        passedCourses.Add(c);
                    }
                }
                //var currCourses = _context.StudentCourses.Where(x => x.StudentId == student.Id && x.IsPassed == false);
                var currentCoursesId = _context.StudentCourses.Where(x => x.StudentId == student.Id && x.IsPassed == false && x.ProgramId == course.StudyProgramId).Select(x => x.CourseId);
                var currentCourses = new List<Course>();

                if (currentCoursesId.Any())
                {
                    foreach (var id in currentCoursesId)
                    {
                        var c = _context.Courses.FirstOrDefault(x => x.Id == id);
                        currentCourses.Add(c);
                    }
                }
                //check if we have course that's not passed with the same programId - we cannot have two curr courses from one program
                if (!currentCourses.Any())
                {
                    //check if we have already passed this course
                    if (!passedCourses.Contains(course) && passedCourses.Any())
                    {
                        //check if it is the next course
                        var f = false;
                        foreach (var passedCourse in passedCourses)
                        {
                            //check if some of passed courses has the new courseId as nextCourseId
                            //if yes - then we add the course as curr
                            if (passedCourse.NextCourseId == courseId)
                            {
                                var currCourse = new StudentCourses
                                {
                                    StudentId = student.Id,
                                    CourseId = course.Id,
                                    IsPassed = false,
                                    ProgramId = course.StudyProgramId
                                };

                                await _context.StudentCourses.AddAsync(currCourse);
                                await _context.SaveChangesAsync();

                                f = true;
                            }
                        }

                        //suggest the proper next course
                        if (!f)
                        {
                            return RedirectToAction("AllCourses", new { studyProgramId = course.StudyProgramId, message = $"You have to continue with {_courseService.GetNextCourseSuggestion(student, course.StudyProgramId)} course" });
                        }
                    }
                    else if (!passedCourses.Any())
                    {
                        //check if it is the first course
                        if (course.ParetntId == 0)
                        {
                            var currCourse = new StudentCourses
                            {
                                StudentId = student.Id,
                                CourseId = course.Id,
                                IsPassed = false,
                                ProgramId = course.StudyProgramId
                            };

                            await _context.StudentCourses.AddAsync(currCourse);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            return RedirectToAction("AllCourses", new { studyProgramId = course.StudyProgramId, message = $"You have to continue with {_courseService.GetNextCourseSuggestion(student, course.StudyProgramId)} course" });
                        }
                    }
                    else
                    {
                        //the course is already passed
                        //return RedirectToAction("AllCourses", new { studyProgramId = course.StudyProgramId, message = $"You have already passed {course.Name} course" });
                        return RedirectToAction("Index", new { courseId = courseId });
                    }
                }
                else
                {
                    //we already have curr course from the same program
                    if (currentCourses.Contains(course))
                    {
                        return RedirectToAction("Index", new { courseId = courseId });
                    }
                    else if (!passedCourses.Contains(course))
                    {
                        return RedirectToAction("AllCourses", new { studyProgramId = course.StudyProgramId, message = $"You are already assigned to course from this program" });
                    }
                }
            }

            return RedirectToAction("Index", new { courseId = courseId });
        }
        public IActionResult Edit(int courseId)
        {
            //TO DO..
            //Add or Change criterias who and when can see the lectures
            //Add remove course for admins only

            var editViewModel = new EditCourseViewModel
            {
                Course = _context.Courses.FirstOrDefault(x => x.Id == courseId),
                Lectures = _context.Lectures.Where(l => l.CourseId == courseId).ToList(),
                SkippingCourseMaterials = _context.LectureFiles.Where(x => x.CourseId == courseId && x.FileTitle == "SKIPPINGEXAM" && x.LectureId == null).ToList()
            };

            var textMaterial = new List<LectureFile>();

            foreach (var lecture in editViewModel.Lectures)
            {
                textMaterial = _context.LectureFiles.Where(l => l.LectureId == lecture.Id).ToList();

                foreach (var file in textMaterial)
                {
                    lecture.TextMaterials.Add(file);
                }
            }

            editViewModel.Course.Lectures = editViewModel.Lectures;
            editViewModel.Course.SkippingCourseMaterials = _context.LectureFiles.Where(x => x.CourseId == courseId && x.FileTitle == "SKIPPINGEXAM" && x.LectureId == null).ToList();

            return View(editViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditCourseViewModel input)
        {
            //TO DO..
            //Change description/grade/skip course end date/ criterias who and when can see lectures
            var course = _context.Courses.FirstOrDefault(x => x.Id == input.CourseId);
            if (input.CourseDescription != null)
            {
                course.Description = input.CourseDescription;
            }

            if (input.CourseStartDate != Convert.ToDateTime("1.1.0001 г. 0:00:00"))
            {
                course.CourseStartDate = input.CourseStartDate;
            }

            if (input.SkipCoursEndDate != Convert.ToDateTime("1.1.0001 г. 0:00:00"))
            {
                course.SkipCoursEndDate = input.SkipCoursEndDate;
            }

            if (input.SkippingCourseFiles != null)
            {
                await _lectureService.CreateLectureFile(null, input.SkippingCourseFiles, "SKIPPINGEXAM", course);
            }

            if (input.RequiredSkippingCourseGrade != 0 && input.RequiredSkippingCourseGrade != course.RequiredSkippingCourseGrade)
                course.RequiredSkippingCourseGrade = input.RequiredSkippingCourseGrade;

            await _context.SaveChangesAsync();
            await _lectureService.EditLecture(input);

            return RedirectToAction("Edit", new { courseId = input.CourseId });
        }
        public async Task<IActionResult> AddSkippingExamSolution(IFormFile file, int courseId)
        {
            var user = await _userManager.GetUserAsync(this.User);
            var currCourse = await _context.Courses.FirstOrDefaultAsync(x => x.Id == courseId);

            await _courseService.AddSkippingExamSolutionToCourse(courseId, file, user.Id);

            this.TempData["Message"] = "Skipping exam solution added successfully!";

            return RedirectToAction("Index", "Course", new { courseId = courseId });
        }

        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> Delete(int courseId)
        {
            var lectures = _context.Lectures.Where(l => l.CourseId == courseId).ToArray();

            await _courseService.DeleteSkippingCourseMaterial(courseId);
            await _courseService.DeleteSkippingAssignment(courseId);

            foreach (var lecture in lectures)
            {
                await _lectureService.DeleteLectureMaterial(lecture.Id);
                await _lectureService.DeleteHomework(lecture.Id);
                await _lectureService.DeleteLecture(lecture.Id);
            }

            await _courseService.DeleteCourse(courseId);

            return RedirectToAction("Index", "Lector");
        }
    }
}
