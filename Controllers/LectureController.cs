﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index()
        {
            return View();
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
          
            await _lectureService.CreateLectureAsync(input, user);

            this.TempData["Message"] = "Lecture created successfully!";

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> AddExamSolution(IFormFile file, int lectureId)
        {
            var user = await _userManager.GetUserAsync(this.User);
            var currLecture = await _context.Lectures.FirstOrDefaultAsync(x => x.Id == lectureId);

            await _lectureService.AddExamSolutionToLecture(lectureId, file, user.Id);

            this.TempData["Message"] = "Homework added successfully!";


            //return RedirectToAction("Index", "Home");
            return RedirectToAction("Index", "Course", new { courseId = 1 });
        }

        public async Task<IActionResult> AddHomework(IFormFile file, int lectureId)
        {
            var user = await _userManager.GetUserAsync(this.User);
            var currLecture = await _context.Lectures.FirstOrDefaultAsync(x => x.Id == lectureId);

            await _lectureService.AddHomeworkToLecture(lectureId, file, user.Id);

            this.TempData["Message"] = "Homework added successfully!";

            return RedirectToAction("Index", "Course", new { courseId = 1 });
        }

        //public async Task<IActionResult> EditHomework(int homeworkId, decimal homeworkGrade, string homeworkComment)
        public async Task<IActionResult> EditHomework(AssessmentsViewModel input)
        {
            await _lectureService.EditHomework(input.HomeworkId, input.HomeworkGrade, input.HomeworkComment);

            //TO DO..
            var hw = await _context.Homeworks.FirstOrDefaultAsync(x => x.Id == input.HomeworkId);
            var student = await _context.Students.FirstOrDefaultAsync(x => x.UserId == hw.StudentId);

            //get all EXAM lectures and all HW for this lecturesID
            //var allFinalLectures = _context.Lectures.Where(x => x.isExam == true).ToList();
            //var allFinalHomeworkSolutions = new List<Homework>();
            //foreach (var lecture in allFinalLectures)
            //{
            //    var homeworks = _context.Homeworks.FirstOrDefault(x => x.LectureId == lecture.Id && x.StudentId == student.UserId);
            //    if (lecture.RequiredGrade <= homeworks.Grade)
            //    {

            //    }
            //    allFinalHomeworkSolutions.Add(homeworks);
            //}


            

            if (hw.HomeworkTitle.ToUpper() == "EXAM")
            {
                //move curr course as passed 
                var passedCourse = new StudentCourses
                {
                    StudentId = student.Id,
                    CourseId = input.CourseId,
                    FinalGrade = input.HomeworkGrade,
                    IsPassed = true
                };

                //Check criteria for passing course
                await _context.StudentCourses.AddAsync(passedCourse);
                await _context.SaveChangesAsync();

                student.CurrentCourseId = null;

                _context.SaveChanges();
            }

            return RedirectToAction("Assessment", "Lector");
        }

        //public async Task<IActionResult> EditLecture(int lectureId, string lectureDescription, int courseId)
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
        
        public async Task<IActionResult> DeleteHomework(int lectureId, int courseId)
        {
            await _lectureService.DeleteHomework(lectureId);
            return RedirectToAction("Edit", "Course", new { courseId = courseId });
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
