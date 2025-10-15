using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MunicipalServiceApp_PROG7312.Models
{
    /// <summary>
    /// Core Event model for municipal events and announcements
    /// POE Part 2 Implementation
    /// </summary>
    public class Event
    {
        public string EventId { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        public string Location { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        /// <summary>
        /// Priority level for Priority Queue (1 = Highest, 5 = Lowest)
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Set of unique tags for categorization and search
        /// Demonstrates Set data structure usage
        /// </summary>
        public HashSet<string> Tags { get; set; } = new HashSet<string>();

        public DateTime CreatedDate { get; set; }
        public bool IsFeatured { get; set; }
        public int AttendeeCount { get; set; }

        public Event()
        {
            EventId = GenerateEventId();
            CreatedDate = DateTime.Now;
            Priority = 3; // Default medium priority
        }

        private static string GenerateEventId()
        {
            return "EVT" + DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(100, 999);
        }
    }

    /// <summary>
    /// View Model for Event Search and Display
    /// Demonstrates use of multiple data structures
    /// </summary>
    public class EventSearchViewModel
    {
        // Search parameters
        public string? SearchQuery { get; set; }
        public string? SelectedCategory { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Categories list for dropdown (from Set)
        public List<string> Categories { get; set; } = new List<string>();

        /// <summary>
        /// Events organized by date using SortedDictionary
        /// Demonstrates SortedDictionary data structure (15 marks)
        /// </summary>
        public SortedDictionary<DateTime, List<Event>> EventsByDate { get; set; }
            = new SortedDictionary<DateTime, List<Event>>();

        /// <summary>
        /// Recommended events based on search patterns
        /// Demonstrates recommendation algorithm (30 marks)
        /// </summary>
        public List<Event> RecommendedEvents { get; set; } = new List<Event>();

        /// <summary>
        /// Recent searches using Queue (FIFO)
        /// Demonstrates Queue data structure (15 marks)
        /// </summary>
        public Queue<string> RecentSearches { get; set; } = new Queue<string>();

        /// <summary>
        /// All matching events
        /// </summary>
        public List<Event> AllEvents { get; set; } = new List<Event>();

        /// <summary>
        /// Search statistics
        /// </summary>
        public SearchStatistics Statistics { get; set; } = new SearchStatistics();

        // UI State
        public int TotalEvents { get; set; }
        public int FilteredEvents { get; set; }
        public string ViewMode { get; set; } = "grid"; // grid or list
    }

    /// <summary>
    /// Search statistics and analytics
    /// </summary>
    public class SearchStatistics
    {
        public int TotalSearches { get; set; }
        public int UniqueCategories { get; set; }
        public string MostSearchedCategory { get; set; } = string.Empty;
        public Dictionary<string, int> CategoryFrequency { get; set; } = new Dictionary<string, int>();
    }

    /// <summary>
    /// User search pattern for recommendation engine
    /// </summary>
    public class UserSearchPattern
    {
        public string SearchTerm { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public DateTime SearchDate { get; set; }
        public int Frequency { get; set; }
    }

    /// <summary>
    /// Event announcement for priority queue
    /// </summary>
    public class EventAnnouncement
    {
        public Event Event { get; set; } = new Event();
        public int Priority { get; set; }
        public DateTime AnnouncementDate { get; set; }
        public bool IsUrgent { get; set; }
    }
}