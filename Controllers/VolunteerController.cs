using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Mvc;

namespace GiftOfTheGivers.Controllers
{
    public class VolunteerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VolunteerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Submit(Volunteer volunteer)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", volunteer);
            }

            _context.Volunteers.Add(volunteer);
            _context.SaveChanges();

            TempData["VolunteerMessage"] =
                "Thank you for registering your interest as a volunteer.";

            return RedirectToAction("Success");
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}