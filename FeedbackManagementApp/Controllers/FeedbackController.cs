using Microsoft.AspNetCore.Mvc;
using FeedbackManagementApp.Models;
using System.Collections.Concurrent;

namespace FeedbackManagementApp.Controllers
{
    public class FeedbackController : Controller
    {
        // Thread-safe in-memory store for Practical purposes
        private static readonly ConcurrentBag<Feedback> _feedbacks = new()
        {
            new Feedback
            {
                Id = 1,
                FullName = "Nihal",
                Email = "nihal@example.com",
                Category = "UI/UX Experience",
                Rating = 5,
                Comments = "The interface looks exceptionally crisp, fast, and smooth to navigate!",
                SubmittedAt = DateTime.Now.AddHours(-3)
            },
            new Feedback
            {
                Id = 2,
                FullName = "Vivek Athreya",
                Email = "vivek.athreya@example.com",
                Category = "Performance",
                Rating = 4,
                Comments = "Load times are solid, but could use a tiny bit more visual feedback during form submission.",
                SubmittedAt = DateTime.Now.AddHours(-1)
            }
        };

        // GET: Feedback/Index (Displays the Feedbacks & Insights Dashboard)
        [HttpGet]
        public IActionResult Index()
        {
            var list = _feedbacks.OrderByDescending(f => f.SubmittedAt).ToList();
            ViewBag.AverageRating = list.Any(f => f.Rating.HasValue)
    ? list.Where(f => f.Rating.HasValue).Average(f => f.Rating!.Value).ToString("0.0")
    : "0.0";
            return View(list);
        }

        // GET: Feedback/Create (Submission Form)
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Feedback());
        }

        // POST: Feedback/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                feedback.Id = _feedbacks.Count + 1;
                feedback.SubmittedAt = DateTime.Now;
                _feedbacks.Add(feedback);

                TempData["SuccessMessage"] = "Thank you! Your feedback has been recorded successfully.";
                return RedirectToAction(nameof(Index));
            }

            return View(feedback);
        }
    }
}