using System.ComponentModel.DataAnnotations;

namespace FeedbackManagementApp.Models
{
    public class Feedback
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter your full name.")]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "Full name must be between 2 and 60 characters.")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Please provide a valid email address.")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a feedback category.")]
        [Display(Name = "Feedback Category")]
        public string Category { get; set; } = string.Empty;

        // Nullable int? ensures no star is selected by default
        [Required(ErrorMessage = "Please select a star rating.")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5 stars.")]
        [Display(Name = "Overall Rating")]
        public int? Rating { get; set; }

        [Required(ErrorMessage = "Your feedback cannot be empty.")]
        [StringLength(600, MinimumLength = 10, ErrorMessage = "Feedback must contain between 10 and 600 characters.")]
        [Display(Name = "Your Feedback")]
        public string Comments { get; set; } = string.Empty;

        [Display(Name = "Submitted On")]
        public DateTime SubmittedAt { get; set; } = DateTime.Now;
    }
}