using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartCampusPortal.Data;
using SmartCampusPortal.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCampusPortal.Controllers
{
    [Route("api/campus")]
    [ApiController]
    public class CampusApiController : ControllerBase
    {
        private readonly CampusDbContext _context;

        public CampusApiController(CampusDbContext context)
        {
            _context = context;
        }

        // GET: /api/campus/requests?category=All&status=All&search=wifi
        [HttpGet("requests")]
        public async Task<IActionResult> GetServiceRequests(
            [FromQuery] string? category = "All",
            [FromQuery] string? status = "All",
            [FromQuery] string? search = "")
        {
            var query = _context.ServiceRequests.AsQueryable();

            if (!string.IsNullOrEmpty(category) && category != "All")
                query = query.Where(r => r.Category == category);

            if (!string.IsNullOrEmpty(status) && status != "All")
                query = query.Where(r => r.Status == status);

            if (!string.IsNullOrEmpty(search))
            {
                string term = search.ToLower();
                query = query.Where(r =>
                    r.RequesterName.ToLower().Contains(term) ||
                    r.CampusId.ToLower().Contains(term) ||
                    r.Location.ToLower().Contains(term) ||
                    r.Description.ToLower().Contains(term));
            }

            var results = await query.OrderByDescending(r => r.CreatedAt).ToListAsync();
            return Ok(results);
        }

        // GET: /api/campus/stats
        [HttpGet("stats")]
        public async Task<IActionResult> GetDashboardStats()
        {
            var total = await _context.ServiceRequests.CountAsync();
            var pending = await _context.ServiceRequests.CountAsync(r => r.Status == "Pending");
            var inProgress = await _context.ServiceRequests.CountAsync(r => r.Status == "In Progress");
            var resolved = await _context.ServiceRequests.CountAsync(r => r.Status == "Resolved");
            var critical = await _context.ServiceRequests.CountAsync(r => r.Priority == "Critical" && r.Status != "Resolved");

            return Ok(new
            {
                total,
                pending,
                inProgress,
                resolved,
                critical
            });
        }

        // PATCH: /api/campus/requests/5/status
        [HttpPatch("requests/{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] StatusUpdateDto dto)
        {
            var request = await _context.ServiceRequests.FindAsync(id);
            if (request == null)
                return NotFound(new { message = $"Ticket #{id} not found." });

            request.Status = dto.Status;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = $"Ticket #{id} status updated to {dto.Status}",
                requestId = request.RequestId,
                newStatus = request.Status
            });
        }

        // DELETE: /api/campus/requests/5
        [HttpDelete("requests/{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var request = await _context.ServiceRequests.FindAsync(id);
            if (request == null)
                return NotFound();

            _context.ServiceRequests.Remove(request);
            await _context.SaveChangesAsync();
            return Ok(new { message = $"Ticket #{id} deleted." });
        }
    }

    public class StatusUpdateDto
    {
        public string Status { get; set; } = string.Empty;
    }
}