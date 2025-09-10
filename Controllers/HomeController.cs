using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MunicipalServiceApp_PROG7312.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MunicipalServiceApp_PROG7312.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        // Using LinkedList instead of List as per POE requirements
        private static LinkedList<Issue> _issueLinkedList = new LinkedList<Issue>();
        private static Stack<Issue> _recentIssuesStack = new Stack<Issue>();
        private static Queue<Issue> _processingQueue = new Queue<Issue>();

        public HomeController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            ViewBag.Title = "Municipal Services Portal";
            return View();
        }

        public IActionResult ReportIssue()
        {
            var model = new ReportIssueViewModel
            {
                Categories = GetIssueCategories(),
                ProgressPercentage = 0,
                EngagementMessage = "Your civic participation makes a difference! Start reporting to improve our community."
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ReportIssue(ReportIssueViewModel model)
        {
            model.Categories = GetIssueCategories();

            if (ModelState.IsValid)
            {
                try
                {
                    var issue = new Issue
                    {
                        IssueId = GenerateIssueId(),
                        Location = model.Location ?? string.Empty,
                        Category = model.Category ?? string.Empty,
                        Description = model.Description ?? string.Empty,
                        DateReported = DateTime.Now,
                        Status = "Submitted"
                    };

                    // Handle file upload
                    if (model.MediaFile != null && model.MediaFile.Length > 0)
                    {
                        var fileName = Path.GetFileName(model.MediaFile.FileName);
                        var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

                        if (!Directory.Exists(uploads))
                        {
                            Directory.CreateDirectory(uploads);
                        }

                        var filePath = Path.Combine(uploads, issue.IssueId + "_" + fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            model.MediaFile.CopyTo(fileStream);
                        }
                        issue.AttachedFilePath = fileName;
                    }

                    // Store using LinkedList (as per POE requirements)
                    _issueLinkedList.AddLast(issue);

                    // Add to Stack for recent tracking
                    _recentIssuesStack.Push(issue);

                    // Add to Queue for processing order
                    _processingQueue.Enqueue(issue);

                    TempData["SuccessMessage"] = $"Issue submitted successfully! Issue ID: {issue.IssueId}";

                    return RedirectToAction("IssueSubmitted", new { id = issue.IssueId });
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "An error occurred while submitting your issue. Please try again.");
                }
            }

            UpdateEngagementProgress(model);
            return View(model);
        }

        public IActionResult IssueSubmitted(string id)
        {
            var issue = GetIssueById(id);
            if (issue == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Issue = issue;
            ViewBag.CategoryCount = CountIssuesInCategory(issue.Category);
            ViewBag.LastSubmitted = _recentIssuesStack.Count > 0 ? _recentIssuesStack.Peek().IssueId : "None";
            ViewBag.NextForProcessing = _processingQueue.Count > 0 ? _processingQueue.Peek().IssueId : "None";

            return View();
        }

        [HttpGet]
        public IActionResult GetProgressUpdate(string location, string category, string description)
        {
            int progress = CalculateProgress(location, category, description);
            string message = GetEngagementMessage(progress);

            var stats = new
            {
                TotalIssues = _issueLinkedList.Count,
                PendingProcessing = _processingQueue.Count
            };

            return Json(new { progress = progress, message = message, stats = stats });
        }

        [HttpGet]
        public IActionResult GetSystemStats()
        {
            return Json(new
            {
                TotalIssues = _issueLinkedList.Count,
                PendingProcessing = _processingQueue.Count
            });
        }

        // Disabled features as per POE Part 1 requirements
        public IActionResult LocalEvents()
        {
            ViewBag.Message = "This feature will be implemented in Part 2 of the POE.";
            ViewBag.IsDisabled = true;
            return View();
        }

        public IActionResult ServiceStatus()
        {
            ViewBag.Message = "This feature will be implemented in Part 3 of the POE.";
            ViewBag.IsDisabled = true;
            return View();
        }

        private List<string> GetIssueCategories()
        {
            return new List<string>
            {
                "Roads and Infrastructure",
                "Water and Sanitation",
                "Electricity",
                "Waste Management",
                "Parks and Recreation",
                "Public Safety",
                "Housing",
                "Traffic and Transportation",
                "Other"
            };
        }

        private string GenerateIssueId()
        {
            return "ISS" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        private Issue? GetIssueById(string id)
        {
            return _issueLinkedList.FirstOrDefault(i => i.IssueId == id);
        }

        private int CountIssuesInCategory(string category)
        {
            return _issueLinkedList.Count(i => i.Category == category);
        }

        private void UpdateEngagementProgress(ReportIssueViewModel model)
        {
            model.ProgressPercentage = CalculateProgress(model.Location, model.Category, model.Description);
            model.EngagementMessage = GetEngagementMessage(model.ProgressPercentage);
        }

        private int CalculateProgress(string? location, string? category, string? description)
        {
            int completedFields = 0;
            int totalFields = 3;

            if (!string.IsNullOrWhiteSpace(location)) completedFields++;
            if (!string.IsNullOrWhiteSpace(category)) completedFields++;
            if (!string.IsNullOrWhiteSpace(description)) completedFields++;

            return (completedFields * 100) / totalFields;
        }

        private string GetEngagementMessage(int progress)
        {
            if (progress == 0)
                return "Your civic participation makes a difference! Start reporting to improve our community.";
            else if (progress < 50)
                return "Great start! Your input helps build a better municipality.";
            else if (progress < 100)
                return "Excellent progress! Your detailed reports help us serve you better.";
            else
                return "Perfect! Your complete report will receive prompt attention from our team.";
        }
    }
}