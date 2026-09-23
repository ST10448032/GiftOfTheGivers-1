using System.Diagnostics;
using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Online()
        {
            return View();
        }

        // Home / Employee Dashboard
        public async Task<IActionResult> Index()
        {
            // Only load statistics for Employees
            if (User.IsInRole("Employee"))
            {
                // Total Volunteers
                ViewBag.TotalVolunteers =
                    await _context.Volunteers.CountAsync();

                // Total Relief Projects
                ViewBag.TotalReliefProjects =
                    await _context.ReliefProjects.CountAsync();

                // Total Donations
                ViewBag.TotalDonations =
                    await _context.Donations.CountAsync();

                // Total ZAR Donations
                ViewBag.TotalZarDonations =
                    await _context.Donations
                        .Where(d => d.Currency == "ZAR")
                        .SumAsync(d => d.Amount);
            }

            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        // GET: Donation page
        [HttpGet]
        [Authorize]
        public IActionResult Donate()
        {
            return View();
        }

        // POST: Save donation
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Donate(Donation donation)
        {
            if (!ModelState.IsValid)
            {
                return View(donation);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }

            // Connect donation to logged-in user
            donation.UserID = user.Id;

            // Save date
            donation.DonationDate = DateTime.Now;

            // Generate unique reference number
            donation.ReferenceNumber =
                "DON-" + Guid.NewGuid().ToString("N")[..8].ToUpper();

            // Save donation
            _context.Donations.Add(donation);

            await _context.SaveChangesAsync();

            // Immediately open the tax certificate
            return RedirectToAction(
                "TaxCertificate",
                new { id = donation.DonationID });
        }

        // GET: Volunteer page
        [HttpGet]
        public IActionResult Volunteer()
        {
            return View();
        }

        // POST: Register volunteer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Volunteer(Volunteer volunteer)
        {
            if (!ModelState.IsValid)
            {
                return View(volunteer);
            }

            _context.Volunteers.Add(volunteer);

            await _context.SaveChangesAsync();

            TempData["VolunteerSuccess"] =
                "Thank you for registering your interest as a volunteer.";

            return RedirectToAction(nameof(Volunteer));
        }

        // Display tax certificate for the donation just made
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> TaxCertificate(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }

            var donation = await _context.Donations
                .FirstOrDefaultAsync(d =>
                    d.DonationID == id &&
                    d.UserID == user.Id);

            if (donation == null)
            {
                return NotFound();
            }

            ViewBag.DonorEmail = user.Email;
            ViewBag.DonorName = user.UserName;

            return View(donation);
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error()
        {
            return View(
                new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id
                               ?? HttpContext.TraceIdentifier
                });
        }
    }
}