using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartCampusPortal.Data;
using SmartCampusPortal.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCampusPortal.Controllers
{
    public class ServiceRequestController : Controller
    {
        private readonly CampusDbContext _context;

        public ServiceRequestController(CampusDbContext context)
        {
            _context = context;
        }

        // GET: /ServiceRequest
        public async Task<IActionResult> Index()
        {
            var requests = await _context.ServiceRequests
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
            return View(requests);
        }

        // GET: /ServiceRequest/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /ServiceRequest/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceRequest request)
        {
            if (ModelState.IsValid)
            {
                request.CreatedAt = DateTime.Now;
                request.Status = "Pending";
                _context.Add(request);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Service Ticket #{request.RequestId} logged successfully for {request.Category}!";
                return RedirectToAction(nameof(Index));
            }
            return View(request);
        }
    }
}