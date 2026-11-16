using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.API.Data;
using StudentManagement.API.Models;
using StudentManagement.API.Models.DTOs;

namespace StudentManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(AppDbContext context, ILogger<StudentsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/students or api/students?department=CSE
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetAll([FromQuery] string? department)
        {
            var query = _context.Students.AsQueryable();

            if (!string.IsNullOrWhiteSpace(department))
            {
                query = query.Where(s => s.Department.ToLower() == department.Trim().ToLower());
            }

            var students = await query.ToListAsync();
            return Ok(students);
        }

        // GET: api/students/1
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Student>> GetById(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound(new { message = $"Student with ID {id} was not found." });
            }

            return Ok(student);
        }

        // POST: api/students
        [HttpPost]
        public async Task<ActionResult<Student>> Create([FromBody] CreateStudentDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Prevent duplicate roll numbers
            bool rollExists = await _context.Students.AnyAsync(s => s.RollNo.ToLower() == dto.RollNo.Trim().ToLower());
            if (rollExists)
            {
                return Conflict(new { message = $"A student with Roll No '{dto.RollNo}' already exists." });
            }

            var student = new Student
            {
                RollNo = dto.RollNo.Trim().ToUpper(),
                FullName = dto.FullName.Trim(),
                Email = dto.Email.Trim().ToLower(),
                Department = dto.Department.Trim().ToUpper(),
                AcademicYear = dto.AcademicYear,
                EnrolledDate = DateTime.UtcNow
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            // Returns 201 Created with Location header referencing GetById
            return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);
        }

        // PUT: api/students/1
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateStudentDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound(new { message = $"Student with ID {id} was not found." });
            }

            student.FullName = dto.FullName.Trim();
            student.Email = dto.Email.Trim().ToLower();
            student.Department = dto.Department.Trim().ToUpper();
            student.AcademicYear = dto.AcademicYear;

            await _context.SaveChangesAsync();

            return NoContent(); // 204 No Content
        }

        // DELETE: api/students/1
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound(new { message = $"Student with ID {id} was not found." });
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent(); // 204 No Content
        }
    }
}