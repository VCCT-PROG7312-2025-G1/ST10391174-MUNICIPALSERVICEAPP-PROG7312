using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MunicipalServiceApp_PROG7312.Models;
using MunicipalServiceApp_PROG7312.Services;
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

                    // Store using LinkedList (Part 1 requirement)
                    _issueLinkedList.AddLast(issue);
                    _recentIssuesStack.Push(issue);
                    _processingQueue.Enqueue(issue);

                    // ? NEW: Also add to Service Request system (Part 3)
                    var serviceRequest = ConvertIssueToServiceRequest(issue);
                    ServiceRequestService.AddServiceRequest(serviceRequest);

                    TempData["SuccessMessage"] = $"Issue submitted successfully! Request ID: {issue.IssueId}";

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

        // ? NEW: Add this helper method to HomeController.cs
        private ServiceRequest ConvertIssueToServiceRequest(Issue issue)
        {
            // Map Priority based on keywords
            var priority = RequestPriority.Medium; // Default
            var description = (issue.Description ?? "").ToLower();

            if (description.Contains("emergency") || description.Contains("urgent") ||
                description.Contains("critical") || description.Contains("danger"))
            {
                priority = RequestPriority.Critical;
            }
            else if (description.Contains("important") || description.Contains("asap"))
            {
                priority = RequestPriority.High;
            }
            else if (description.Contains("minor") || description.Contains("whenever"))
            {
                priority = RequestPriority.Low;
            }

            var serviceRequest = new ServiceRequest
            {
                RequestId = issue.IssueId, // Use same ID for tracking
                Title = $"{issue.Category} - {issue.Location}",
                Category = issue.Category,
                Description = issue.Description,
                Location = issue.Location,
                Status = RequestStatus.Submitted,
                Priority = priority,
                DateSubmitted = issue.DateReported,
                AttachmentPath = issue.AttachedFilePath ?? string.Empty,
                CitizenId = "CITIZEN_" + DateTime.Now.Ticks // Generate citizen ID
            };

            return serviceRequest;
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

            // ? NEW: Add tracking link info
            ViewBag.CanTrackRequest = true;
            ViewBag.TrackingUrl = Url.Action("TrackRequest", "Home", new { id = issue.IssueId });

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






        /// <summary>
        /// Local Events and Announcements - POE Part 2
        /// Demonstrates advanced data structures and recommendation engine
        /// claude was used to demonstrate how to used advanced data structures
        /// </summary>
        [HttpGet]
        public IActionResult LocalEvents(string searchQuery, string category, DateTime? startDate, DateTime? endDate)
        {
            var model = new EventSearchViewModel
            {
                SearchQuery = searchQuery,
                SelectedCategory = category,
                StartDate = startDate,
                EndDate = endDate,
                Categories = EventService.GetAllCategories()
            };

            // Get filtered events using data structures
            var events = EventService.SearchEvents(searchQuery, category, startDate);

            // Organize by date using SortedDictionary
            model.EventsByDate = EventService.OrganizeEventsByDate(events);
            model.AllEvents = events;

            // Get recommendations using advanced algorithm
            model.RecommendedEvents = EventService.GetRecommendations(searchQuery, category);

            // Get recent searches using Queue
            model.RecentSearches = EventService.GetRecentSearches();

            // Statistics
            model.Statistics = EventService.GetSearchStatistics();
            model.TotalEvents = EventService.GetTotalEvents();
            model.FilteredEvents = events.Count;

            return View(model);
        }

        /// <summary>
        /// AJAX endpoint for real-time recommendations
        /// Chatgpt was used in order to create this method that gets event recommendations
        /// </summary>
        [HttpGet]
        public IActionResult GetEventRecommendations(string searchQuery, string category)
        {
            var recommendations = EventService.GetRecommendations(searchQuery, category, 6);

            return Json(new
            {
                success = true,
                recommendations = recommendations.Select(e => new
                {
                    eventId = e.EventId,
                    title = e.Title,
                    category = e.Category,
                    eventDate = e.EventDate.ToString("yyyy-MM-dd HH:mm"),
                    location = e.Location,
                    isFeatured = e.IsFeatured,
                    priority = e.Priority
                })
            });
        }

        /// <summary>
        /// Get featured events using Priority Queue
        /// </summary>
        [HttpGet]
        public IActionResult GetFeaturedEvents()
        {
            var featured = EventService.GetFeaturedEvents(5);

            return Json(new
            {
                success = true,
                events = featured.Select(e => new
                {
                    eventId = e.EventId,
                    title = e.Title,
                    category = e.Category,
                    eventDate = e.EventDate.ToString("yyyy-MM-dd HH:mm"),
                    location = e.Location
                })
            });
        }

        /// <summary>
        /// Get search statistics
        /// used claude AI to guide on how to get search statistics
        /// </summary>
        [HttpGet]
        public IActionResult GetEventStatistics()
        {
            var stats = EventService.GetSearchStatistics();

            return Json(new
            {
                success = true,
                totalSearches = stats.TotalSearches,
                uniqueCategories = stats.UniqueCategories,
                mostSearchedCategory = stats.MostSearchedCategory,
                totalEvents = EventService.GetTotalEvents(),
                upcomingEvents = EventService.GetUpcomingEventsCount()
            });
        }

        /// <summary>
        /// Track search patterns
        /// </summary>
        [HttpPost]
        public IActionResult TrackSearch([FromBody] SearchTrackingRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                EventService.TrackSearch(request.SearchTerm);
            }

            if (!string.IsNullOrWhiteSpace(request.Category))
            {
                EventService.TrackCategorySearch(request.Category);
            }

            return Json(new { success = true });
        }
     
// Main status page with search/filter
        /// <summary>
        /// Service Request Status Page - POE Part 3
        /// Demonstrates BST, AVL Tree, Heaps, and Graphs
        /// </summary>
[HttpGet]
        public IActionResult ServiceStatus(string searchQuery, string filterStatus,
    string filterCategory, RequestPriority? filterPriority)
        {
            var model = new ServiceRequestStatusViewModel
            {
                SearchQuery = searchQuery,
                FilterStatus = filterStatus,
                FilterCategory = filterCategory,
                FilterPriority = filterPriority
            };

            // Get all requests using BST traversal
            model.AllRequests = ServiceRequestService.GetAllRequests();

            // Apply filters using BST search
            model.FilteredRequests = ServiceRequestService.SearchRequests(
                searchQuery, filterStatus, filterCategory, filterPriority);

            // Get statistics
            model.TotalRequests = ServiceRequestService.GetTotalRequests();
            model.PendingRequests = ServiceRequestService.GetPendingRequests();
            model.CompletedRequests = ServiceRequestService.GetCompletedRequests();
            model.InProgressRequests = model.AllRequests.Count(r =>
                r.Status == RequestStatus.InProgress);

            // Group by status using tree traversal
            model.RequestsByStatus = ServiceRequestService.GetRequestCountByStatus();

            // Group by category
            model.RequestsByCategory = ServiceRequestService.GetRequestCountByCategory();

            // Get dependency graph for visualization
            model.DependencyGraph = ServiceRequestService.GetAllDependencies();

            // Build related requests tree
            foreach (var request in model.FilteredRequests)
            {
                var related = ServiceRequestService.GetRelatedRequests(request.RequestId);
                if (related.Any())
                {
                    model.RelatedRequestsTree[request.RequestId] = related;
                }
            }

            return View(model);
        }

        /// <summary>
        /// Track specific request with detailed information
        /// </summary>
        [HttpGet]
        public IActionResult TrackRequest(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["ErrorMessage"] = "Please provide a valid request ID.";
                return RedirectToAction("ServiceStatus");
            }

            var request = ServiceRequestService.GetRequestById(id);
            if (request == null)
            {
                TempData["ErrorMessage"] = $"Request '{id}' not found.";
                return RedirectToAction("ServiceStatus");
            }

            var model = new TrackRequestViewModel
            {
                Request = request,
                RelatedRequests = ServiceRequestService.GetRelatedRequests(id),
                Timeline = request.StatusHistory.OrderBy(s => s.Timestamp).ToList(),
                ProgressPercentage = request.GetCompletionProgress()
            };

            // Calculate estimated completion time
            if (request.Status != RequestStatus.Completed)
            {
                var avgCompletionDays = ServiceRequestService.GetAverageCompletionTime();
                var daysSinceSubmission = (DateTime.Now - request.DateSubmitted).TotalDays;
                model.EstimatedDaysToCompletion = Math.Max(0,
                    (int)(avgCompletionDays - daysSinceSubmission));
            }

            // Find similar requests using BST search
            model.SimilarRequests = ServiceRequestService.SearchRequests(
                null, null, request.Category, null)
                .Where(r => r.RequestId != id)
                .Take(5)
                .ToList();

            return View(model);
        }
        /// <summary>
        /// Get high priority requests using Min Heap
        /// </summary>
        [HttpGet]
        public IActionResult GetPriorityRequests(int count = 10)
        {
            var priorityRequests = ServiceRequestService.GetHighPriorityRequests(count);

            return Json(new
            {
                success = true,
                requests = priorityRequests.Select(r => new
                {
                    requestId = r.RequestId,
                    title = r.Title,
                    category = r.Category,
                    priority = r.Priority.ToString(),
                    status = r.Status.ToString(),
                    dateSubmitted = r.DateSubmitted.ToString("yyyy-MM-dd HH:mm"),
                    location = r.Location
                })
            });
        }

        /// <summary>
        /// Get request statistics
        /// </summary>
        [HttpGet]
        public IActionResult GetServiceStatistics()
        {
            var stats = new
            {
                total = ServiceRequestService.GetTotalRequests(),
                pending = ServiceRequestService.GetPendingRequests(),
                completed = ServiceRequestService.GetCompletedRequests(),
                averageCompletionDays = ServiceRequestService.GetAverageCompletionTime(),
                byStatus = ServiceRequestService.GetRequestCountByStatus(),
                byCategory = ServiceRequestService.GetRequestCountByCategory()
            };

            return Json(new { success = true, statistics = stats });
        }
        /// <summary>
        /// Search requests by date range using AVL Tree
        /// </summary>
        [HttpGet]
        public IActionResult SearchByDateRange(DateTime startDate, DateTime endDate)
        {
            var requests = ServiceRequestService.GetRequestsByDateRange(startDate, endDate);

            return Json(new
            {
                success = true,
                count = requests.Count,
                requests = requests.Select(r => new
                {
                    requestId = r.RequestId,
                    title = r.Title,
                    dateSubmitted = r.DateSubmitted.ToString("yyyy-MM-dd"),
                    status = r.Status.ToString()
                })
            });
        }

        /// <summary>
        /// Get dependency graph data for visualization
        /// </summary>
        [HttpGet]
        public IActionResult GetDependencyGraph()
        {
            var dependencies = ServiceRequestService.GetAllDependencies();

            return Json(new
            {
                success = true,
                dependencies = dependencies.Select(d => new
                {
                    from = d.FromRequestId,
                    to = d.ToRequestId,
                    type = d.Type.ToString()
                })
            });
        }
        // Helper class for search tracking
        public class SearchTrackingRequest
        {
            public string? SearchTerm { get; set; }
            public string? Category { get; set; }
        }


    }
}
