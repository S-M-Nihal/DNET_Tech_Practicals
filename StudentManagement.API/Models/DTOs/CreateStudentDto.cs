using System.ComponentModel.DataAnnotations;

namespace StudentManagement.API.Models.DTOs
{
    public class CreateStudentDto
    {
        [Required(ErrorMessage = "Roll number is required.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Roll number must be between 3 and 20 characters.")]
        public string RollNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(100, ErrorMessage = "Full Name cannot exceed 100 characters.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please provide a valid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Department is required.")]
        public string Department { get; set; } = string.Empty;

        [Range(1, 4, ErrorMessage = "Academic year must be between 1 and 4.")]
        public int AcademicYear { get; set; }
    }
}