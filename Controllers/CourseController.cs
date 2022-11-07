﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMUniGraduation.Data;
using MMUniGraduation.Models;
using MMUniGraduation.Models.Create;
using MMUniGraduation.Services.Interfaces;
using MMUniGraduation.ViewModels;
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
            else
            {
                path = Path.Combine(_webHost.WebRootPath, "files/") + fileName;
            }
            //string path = Path.Combine(_webHost.WebRootPath, "files/") + fileName;

            byte[] bytes = System.IO.File.ReadAllBytes(path);

            return File(bytes, "application/octet-stream", fileName);
        }
        //public async Task<IActionResult> Index(int courseId)
        public IActionResult Index(int courseId)
        {
            var currentCourse = _context.Courses.FirstOrDefault(x => x.Id == courseId);
            currentCourse.Lectures = _context.Lectures.Where(l => l.CourseId == courseId).ToList();

            var textMaterial = new List<LectureFile>();

            foreach (var lecture in currentCourse.Lectures)
            {
                textMaterial = _context.LectureFiles.Where(l => l.LectureId == lecture.Id).ToList();

                lecture.TextMaterials = textMaterial;
                //foreach (var file in textMaterial)
                //{
                //    lecture.TextMaterials.Add(file);
                //}
            }

            var user = _userManager.GetUserAsync(this.User);
            var homework = new List<Homework>();
            foreach (var lecture in currentCourse.Lectures)
            {
                homework = _context.Homeworks.Where(l => l.LectureId == lecture.Id).ToList();

                lecture.Homeworks = homework;
            }

            //TO DO..
            this.TempData["Message"] = "You have been sucessfully assigned to this course !";

            return View(currentCourse);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var viewModel = new CreateCourse
            {
                StudyPrograms = _studyProgramService.GetAllAsKeyValuePairs(),
                Courses = _courseService.GetAllAsKeyValuePairs()
            };

            return this.View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateCourse input)
        {
            if (!ModelState.IsValid)
            {
                input.StudyPrograms = _studyProgramService.GetAllAsKeyValuePairs();

                return this.View(input);
            }

            //var user = await _userManager.GetUserAsync(this.User);

            await _courseService.CreateCourseAsync(input);

            this.TempData["Message"] = "Course created successfully!";

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> AllCourses(int studyProgramId)
        {
            var user = await _userManager.GetUserAsync(this.User);
            var viewModel = new AllCoursesViewModel
            {
                NextCourseName = _courseService.GetNextCourseSuggestion(user),
                AllCourses = await _context.Courses.Where(c => c.StudyProgramId == studyProgramId).ToListAsync()
            };
            return View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> AssignUserToCourse(int courseId)
        {
            var user = await _userManager.GetUserAsync(this.User);

            if (user.CurrentCourseId == 0)
            {
                user.CurrentCourseId = courseId;
                _context.SaveChanges();
            }

            return RedirectToAction("Index", new { courseId = courseId });
        }

        public IActionResult Edit(int courseId)
        {
            //TO DO..
            //Add homework/new lecture materiad/homework grade
            //Remove lecture material
            //Change description/grade/skip course end date/ criterias who and when can see lectures

            //var currentCourse = _context.Courses.FirstOrDefault(x => x.Id == courseId);
            //currentCourse.Lectures = _context.Lectures.Where(l => l.CourseId == courseId).ToList();

            var editViewModel = new EditCourseViewModel
            {
                Course = _context.Courses.FirstOrDefault(x => x.Id == courseId),
                Lectures = _context.Lectures.Where(l => l.CourseId == courseId).ToList()
        };

            //editViewModel.Course.Lectures = _context.Lectures.Where(l => l.CourseId == courseId).ToList();
            
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

            return View(editViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditCourseViewModel input)
        {
            //TO DO..
            //Change description/grade/skip course end date/ criterias who and when can see lectures

            var editViewModel = new EditCourseViewModel
            {
                Course = _context.Courses.FirstOrDefault(x => x.Id == input.CourseId),
                Lectures = _context.Lectures.Where(l => l.CourseId == input.CourseId).ToList()
        };
            var currLecture = _context.Lectures.Where(l => l.CourseId == input.CourseId && l.Id == input.LectureId);

            await _lectureService.EditLecture(input);

            //return View(editViewModel);
            return RedirectToAction("Edit", new { courseId = input.CourseId });
        }

        [Authorize(Roles = "Teacher")]
        public IActionResult Delete(int courseId)
        {
            //TO DO..
            //Remove course
            //Return message
            //return this.View();
            return RedirectToAction("Index", "Home");
        }
    }
}
