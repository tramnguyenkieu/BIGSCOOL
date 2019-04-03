using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BigSchool.Models;
using BigSchool.ViewModels;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace BigSchool.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        // GET: Course
        //adsfafa
        ///fsdgrreycghb,jhm
        public CoursesController()
        {
            _dbContext = new ApplicationDbContext();
        }

        // GET: Courses
        [HttpGet]
        public ActionResult Create()
        {
            var viewModel = new CourseViewModel
            {
                Categories = _dbContext.Categories.ToList()
            };
            return View(viewModel);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CourseViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = _dbContext.Categories.ToList();
                return View("Create", viewModel);
            }
            var course = new Course
            {
                LecturerId = User.Identity.GetUserId(),
                Datetime = viewModel.GetDateTime(),
                CategoryId = viewModel.Category,
                Place = viewModel.Place

            };
            _dbContext.Courses.Add(course);
            _dbContext.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();
            var courses = _dbContext.Attendances

              .Where(a => a.AttendeeId == userId)
              .Select(a => a.Course)
              .Include(l => l.Lecturer)
              .Include(l => l.Category)
              .ToList();
            var viewModel = new CoursesViewModel
            {
                UpCommingCourses = courses,
                ShowAction = User.Identity.IsAuthenticated
            };
             
            return View(viewModel);
        }
    }
}