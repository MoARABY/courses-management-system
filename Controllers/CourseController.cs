using FinalProject.EntityF;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace FinalProject.Controllers
{
    public class CourseController : Controller
    {
        AppDbContext _Con;
        public CourseController(AppDbContext _appDbContext)
        {
            _Con = _appDbContext;
        }

        [Authorize(Roles = $"{nameof(UserType.instructur)},{nameof(UserType.Student)}")]
        public IActionResult Index()
        {
            var userid = Convert.ToInt16(User.FindFirstValue("User_ID"));
            var userrole = User.FindFirstValue(ClaimTypes.Role);
            List<CourseDetailsModule> courses;
            if (userrole == nameof(UserType.instructur))
            {
                 courses = _Con.Courses
                .Where(c => c.Ins_ID == userid)
                .Select(c => new CourseDetailsModule
                {
                    Course_Id = c.Course_Id,
                    Course_Name = c.Course_Name,
                    Description = c.Description,
                    StartDate = c.StartDate,
                    Capacity = c.Capacity,
                    Duration = c.Duration,
                    EndDate = c.EndDate,
                }).ToList();
            }
            else 
            {
                courses = (from c in _Con.Courses
                               //  join e in _Con.CourseEnrollments.Where(en => en.Student_ID == userid) on c.Course_Id equals e.Course_ID
                           where c.Enrollments.Any(en=>en.Student_ID == userid)
                           select  new CourseDetailsModule
                           {
                            Course_Id = c.Course_Id,
                            Course_Name = c.Course_Name,
                            Description = c.Description,
                            StartDate = c.StartDate,
                            Capacity = c.Capacity,
                            Duration = c.Duration,
                            EndDate = c.EndDate,
                           }).ToList();
            }
            return View(courses);
        }

        [Authorize(Roles = nameof(UserType.instructur))]
        public IActionResult Create( )
        { 
              return View(); 
        }

        [Authorize(Roles = nameof(UserType.instructur))]
        public IActionResult AddCourse(CreateCourseModule c)
        {

            var State = ModelState;
            var userid = Convert.ToInt16(User.FindFirstValue("User_ID"));
            if (ModelState.IsValid == false)
            { return View("Create",c); }
            var course = new Course()
            {
                Course_Name = c.Course_Name,
                Description = c.Description,
                StartDate = c.StartDate,
                Capacity = c.Capacity,
                Duration = c.Duration,
                EndDate = c.EndDate,
                Ins_ID = userid,
            };
            _Con.Courses.Add(course);
            _Con.SaveChanges(); 
            return RedirectToAction("Index");
        }

        //[Route("Courses/{id}")]
        public IActionResult Details(int id)
        {
            var course = _Con.Courses.FirstOrDefault(c => c.Course_Id == id);
            if(course == null) {

                return NotFound();
            }
            var courseModule = new CourseDetailsModule
            {
                Course_Name = course.Course_Name,
                Description = course.Description,
                StartDate = course.StartDate,
                Capacity = course.Capacity,
                Duration = course.Duration,
                EndDate = course.EndDate,
            };
            return View(courseModule);
        }

        public IActionResult Info()
        {
            return View();
        }

        [Authorize(Roles = nameof(UserType.instructur))]
        [HttpGet]
        public IActionResult Edit(int id)
        {
                var course = _Con.Courses.FirstOrDefault(c => c.Course_Id == id);
                if (course == null)
                {
                    return NotFound();
                }
                {
                return View(course);
                }; ;
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserType.instructur))]
        public IActionResult Edit(Course course)
        {
            if (ModelState.IsValid)
            {
                _Con.Courses.Update(course);
                _Con.SaveChanges();
            //    return RedirectToAction("Index");
            }
                return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = nameof(UserType.Student))]
        public IActionResult Enroll()
        {
            var userid = Convert.ToInt16(User.FindFirstValue("User_ID"));
            var courses = (from c in _Con.Courses
                       where c.Enrollments.Any(en => en.Student_ID == userid)==false
                       && c.Capacity > c.Enrollments.Count
                       && c.StartDate >= DateTime.Now
                       select new CourseDetailsModule
                       {
                           Course_Id = c.Course_Id,
                           Course_Name = c.Course_Name,
                           Description = c.Description, 
                           StartDate = c.StartDate,
                           Capacity = c.Capacity,
                           Duration = c.Duration,
                           EndDate = c.EndDate,
                       }).ToList();
            return View(courses);
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserType.Student))]
        public IActionResult Enroll(int id)
        {
            var userid = Convert.ToInt16(User.FindFirstValue("User_ID"));
            var EnR = new CourseEnrollment { Student_ID = userid, Course_ID = id };
            _Con.CourseEnrollments.Add(EnR);
            _Con.SaveChanges();
            return RedirectToAction("Index");
        }


        public IActionResult Ins()
        {
            var ins = _Con.Instuctors.Select(c => new Instuctor
            {
                Instructur_Name=c.Instructur_Name,
                Phone=c.Phone
            }).ToList();

            return View(ins);
        }

    }
}
