using System;
using System.ComponentModel.DataAnnotations;

namespace SmartCampusPortal.Models
{
    public class ServiceRequest
    {
        [Key]
        public int RequestId { get; set; }

        [Required(ErrorMessage = "Student or Staff Name is required")]
        [StringLength(100)]
        [Display(Name = "Requester Name")]
        public string RequesterName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Roll No or Employee ID is required")]
        [StringLength(30)]
        [Display(Name = "Campus ID / Roll No")]
        public string CampusId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a service category")]
        [StringLength(50)]
        [Display(Name = "Service Category")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Location / Room No is required")]
        [StringLength(100)]
        [Display(Name = "Building & Room No")]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please describe the issue")]
        [StringLength(500)]
        [Display(Name = "Issue Description")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Priority { get; set; } = "Medium";

        [StringLength(30)]
        public string Status { get; set; } = "Pending";

        [Display(Name = "Logged On")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}