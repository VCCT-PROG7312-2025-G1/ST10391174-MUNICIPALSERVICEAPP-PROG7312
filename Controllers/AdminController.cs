using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MunicipalServiceApp_PROG7312.Models;
using MunicipalServiceApp_PROG7312.Services;
using System;
using System.Linq;

namespace MunicipalServiceApp_PROG7312.Controllers
{
    public class AdminController : Controller
    {
        // Session key for admin authentication
        private const string AdminSessionKey = "IsAdminAuthenticated";

        /// <summary>
        /// Admin login page
        /// </summary>
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            // If already logged in, redirect to dashboard
            if (IsAdminAuthenticated())
            {
                return RedirectToAction("Dashboard");
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(new AdminLoginViewModel());
        }

        /// <summary>
        /// Process admin login
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(AdminLoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                if (AdminCredentials.ValidateCredentials(model.Username, model.Password))
                {
                    // Set session to indicate admin is logged in
                    HttpContext.Session.SetString(AdminSessionKey, "true");

                    TempData["SuccessMessage"] = "Welcome, Admin! You have successfully logged in.";

                    // Redirect to return URL or dashboard
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password");
                }
            }

            return View(model);
        }

        /// <summary>
        /// Admin logout
        /// </summary>
        public IActionResult Logout()
        {
            HttpContext.Session.Remove(AdminSessionKey);
            TempData["InfoMessage"] = "You have been logged out successfully.";
            return RedirectToAction("Login");
        }

        /// <summary>
        /// Admin dashboard
        /// </summary>
        public IActionResult Dashboard()
        {
            if (!IsAdminAuthenticated())
            {
                return RedirectToAction("Login", new { returnUrl = Url.Action("Dashboard") });
            }

            var model = new AdminDashboardViewModel
            {
                AllEvents = EventService.GetAllEvents(),
                TotalEvents = EventService.GetTotalEvents(),
                UpcomingEvents = EventService.GetUpcomingEventsCount(),
                FeaturedEvents = EventService.GetAllEvents().Count(e => e.IsFeatured),
                RecentEvents = EventService.GetAllEvents()
                    .OrderByDescending(e => e.CreatedDate)
                    .Take(5)
                    .ToList()
            };

            // Count events by category
            foreach (var evt in model.AllEvents)
            {
                if (model.EventsByCategory.ContainsKey(evt.Category))
                {
                    model.EventsByCategory[evt.Category]++;
                }
                else
                {
                    model.EventsByCategory[evt.Category] = 1;
                }
            }

            return View(model);
        }

        /// <summary>
        /// Create new event form
        /// used chatgpt to create this method for me 
        /// </summary>
        [HttpGet]
        public IActionResult CreateEvent()
        {
            if (!IsAdminAuthenticated())
            {
                return RedirectToAction("Login", new { returnUrl = Url.Action("CreateEvent") });
            }

            var model = new ManageEventViewModel
            {
                AvailableCategories = EventService.GetAllCategories(),
                EventDate = DateTime.Now.AddDays(7),
                Priority = 3
            };

            return View(model);
        }

        /// <summary>
        /// Create new event
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateEvent(ManageEventViewModel model)
        {
            if (!IsAdminAuthenticated())
            {
                return RedirectToAction("Login");
            }

            model.AvailableCategories = EventService.GetAllCategories();

            if (ModelState.IsValid)
            {
                var newEvent = new Event
                {
                    Title = model.Title,
                    Category = model.Category,
                    Description = model.Description,
                    EventDate = model.EventDate,
                    Location = model.Location,
                    Priority = model.Priority,
                    IsFeatured = model.IsFeatured,
                    AttendeeCount = model.AttendeeCount
                };

                // Parse tags
                if (!string.IsNullOrWhiteSpace(model.TagsString))
                {
                    var tags = model.TagsString.Split(',')
                        .Select(t => t.Trim())
                        .Where(t => !string.IsNullOrEmpty(t));

                    foreach (var tag in tags)
                    {
                        newEvent.Tags.Add(tag);
                    }
                }

                EventService.AddEvent(newEvent);

                TempData["SuccessMessage"] = $"Event '{newEvent.Title}' created successfully!";
                return RedirectToAction("Dashboard");
            }

            return View(model);
        }

        /// <summary>
        /// Edit event form
        /// </summary>
        [HttpGet]
        public IActionResult EditEvent(string id)
        {
            if (!IsAdminAuthenticated())
            {
                return RedirectToAction("Login", new { returnUrl = Url.Action("EditEvent", new { id }) });
            }

            var evt = EventService.GetEventById(id);
            if (evt == null)
            {
                TempData["ErrorMessage"] = "Event not found.";
                return RedirectToAction("Dashboard");
            }

            var model = new ManageEventViewModel
            {
                EventId = evt.EventId,
                Title = evt.Title,
                Category = evt.Category,
                Description = evt.Description,
                EventDate = evt.EventDate,
                Location = evt.Location,
                Priority = evt.Priority,
                IsFeatured = evt.IsFeatured,
                AttendeeCount = evt.AttendeeCount,
                TagsString = string.Join(", ", evt.Tags),
                AvailableCategories = EventService.GetAllCategories()
            };

            return View(model);
        }

        /// <summary>
        /// Update event
        /// used chatgpt to help me create this method
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditEvent(ManageEventViewModel model)
        {
            if (!IsAdminAuthenticated())
            {
                return RedirectToAction("Login");
            }

            model.AvailableCategories = EventService.GetAllCategories();

            if (ModelState.IsValid)
            {
                var evt = EventService.GetEventById(model.EventId);
                if (evt == null)
                {
                    TempData["ErrorMessage"] = "Event not found.";
                    return RedirectToAction("Dashboard");
                }

                // Update event properties
                evt.Title = model.Title;
                evt.Category = model.Category;
                evt.Description = model.Description;
                evt.EventDate = model.EventDate;
                evt.Location = model.Location;
                evt.Priority = model.Priority;
                evt.IsFeatured = model.IsFeatured;
                evt.AttendeeCount = model.AttendeeCount;

                // Update tags
                evt.Tags.Clear();
                if (!string.IsNullOrWhiteSpace(model.TagsString))
                {
                    var tags = model.TagsString.Split(',')
                        .Select(t => t.Trim())
                        .Where(t => !string.IsNullOrEmpty(t));

                    foreach (var tag in tags)
                    {
                        evt.Tags.Add(tag);
                    }
                }

                EventService.UpdateEvent(evt);

                TempData["SuccessMessage"] = $"Event '{evt.Title}' updated successfully!";
                return RedirectToAction("Dashboard");
            }

            return View(model);
        }

        /// <summary>
        /// Delete event confirmation
        /// </summary>
        [HttpGet]
        public IActionResult DeleteEvent(string id)
        {
            if (!IsAdminAuthenticated())
            {
                return RedirectToAction("Login");
            }

            var evt = EventService.GetEventById(id);
            if (evt == null)
            {
                TempData["ErrorMessage"] = "Event not found.";
                return RedirectToAction("Dashboard");
            }

            return View(evt);
        }

        /// <summary>
        /// Delete event
        /// </summary>
        [HttpPost, ActionName("DeleteEvent")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteEventConfirmed(string id)
        {
            if (!IsAdminAuthenticated())
            {
                return RedirectToAction("Login");
            }

            var evt = EventService.GetEventById(id);
            if (evt == null)
            {
                TempData["ErrorMessage"] = "Event not found.";
                return RedirectToAction("Dashboard");
            }

            EventService.DeleteEvent(id);

            TempData["SuccessMessage"] = $"Event '{evt.Title}' deleted successfully!";
            return RedirectToAction("Dashboard");
        }

        /// <summary>
        /// Check if admin is authenticated
        /// </summary>
        private bool IsAdminAuthenticated()
        {
            var session = HttpContext.Session.GetString(AdminSessionKey);
            return session == "true";
        }
    }
}