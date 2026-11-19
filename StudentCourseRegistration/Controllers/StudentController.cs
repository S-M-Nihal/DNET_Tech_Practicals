using Microsoft.AspNetCore.Mvc;
using StudentCourseRegistration.Models;

namespace StudentCourseRegistration.Controllers
{
    public class StudentController : Controller
    {
        // Static in-memory list so data persists across HTTP requests while the app runs
        private static readonly List<Student> _students = new()
        {
            new Student
            {
                Id = 1,
                Name = "S.M. Nihal",
                Email = "nihal.92400120306@marwadiuniversity.ac.in",
                Course = ".NET Technologies (01CE1523)",
                Semester = 5
            },
            new Student
            {
                Id = 2,
                Name = "Aarav Patel",
                Email = "aarav.patel@marwadiuniversity.ac.in",
                Course = "Operating Systems",
                Semester = 5
            },
            new Student
            {
                Id = 3,
                Name = "Priya Sharma",
                Email = "priya.sharma@marwadiuniversity.ac.in",
                Course = "Database Management Systems",
                Semester = 4
            }
        };

        // Tracks the next available auto-incrementing ID
        private static int _nextId = 4;

        // GET: /Student or /Student/Index
        [HttpGet]
        public IActionResult Index()
        {
            return View(_students);
        }

        // GET: /Student/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Student student)
        {
            // Verify all Data Annotations on Student passed
            if (!ModelState.IsValid)
            {
                // Return the same form with entered values and validation error messages
                return View(student);
            }

            // Assign auto-incrementing Id and add to in-memory list
            student.Id = _nextId++;
            _students.Add(student);

            // Store one-time confirmation message across the redirect
            TempData["SuccessMessage"] = $"Student '{student.Name}' has been registered successfully!";

            // Redirect to Student/Index (Post-Redirect-Get pattern)
            return RedirectToAction(nameof(Index));
        }

        // GET: /Student/Details/1
        [HttpGet]
        public IActionResult Details(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);

            // Handle non-existent student ID cleanly with HTTP 404
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }
    }
}