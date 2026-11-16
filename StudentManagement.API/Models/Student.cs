using System.ComponentModel.DataAnnotations;

namespace StudentManagement.API.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string RollNo { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Department { get; set; } = string.Empty;

        [Range(1, 4)]
        public int AcademicYear { get; set; }

        public DateTime EnrolledDate { get; set; } = DateTime.UtcNow;
    }
}