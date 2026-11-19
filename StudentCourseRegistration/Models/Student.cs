using System.ComponentModel.DataAnnotations;

namespace StudentCourseRegistration.Models
{
    public class Student
    {
        [Display(Name = "Student ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Student name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters.")]
        [Display(Name = "Full Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address (e.g., name@marwadiuniversity.ac.in).")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a course.")]
        [StringLength(60, ErrorMessage = "Course name cannot exceed 60 characters.")]
        [Display(Name = "Registered Course")]
        public string Course { get; set; } = string.Empty;

        [Required(ErrorMessage = "Semester is required.")]
        [Range(1, 8, ErrorMessage = "Semester must be between 1 and 8.")]
        [Display(Name = "Current Semester")]
        public int Semester { get; set; }
    }
}