using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Controllers
{
    [Authorize]
    public class DonationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DonationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Donation page
        [HttpGet]
        public IActionResult Online()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        // Submit donation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(Donation donation)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", donation);
            }

            donation.DonationDate = DateTime.Now;

            // Generate a reference number for the donation
            donation.ReferenceNumber =
                "DON-" + Guid.NewGuid().ToString("N")[..8].ToUpper();

            // Save the donation
            _context.Donations.Add(donation);

            await _context.SaveChangesAsync();

            // Immediately show the certificate for THIS donation
            return RedirectToAction(
                nameof(Certificate),
                new { id = donation.DonationID });
        }

        // Display the tax certificate for a specific donation
        [HttpGet]
        public async Task<IActionResult> Certificate(int id)
        {
            var donation = await _context.Donations
                .FirstOrDefaultAsync(d => d.DonationID == id);

            if (donation == null)
            {
                return NotFound();
            }

            return View(donation);
        }

        // Display all donations made by the logged-in user
        [HttpGet]
        public async Task<IActionResult> MyDonations()
        {
            var userId = User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var donations = await _context.Donations
                .Where(d => d.UserID == userId)
                .OrderByDescending(d => d.DonationDate)
                .ToListAsync();

            return View(donations);
        }
    }
}