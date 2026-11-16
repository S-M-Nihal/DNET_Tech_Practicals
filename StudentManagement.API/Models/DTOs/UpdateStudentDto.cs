using System.ComponentModel.DataAnnotations;

namespace StudentManagement.API.Models.DTOs
{
    public class UpdateStudentDto
    {
        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Department is required.")]
        public string Department { get; set; } = string.Empty;

        [Range(1, 4, ErrorMessage = "Academic year must be between 1 and 4.")]
        public int AcademicYear { get; set; }
    }
}