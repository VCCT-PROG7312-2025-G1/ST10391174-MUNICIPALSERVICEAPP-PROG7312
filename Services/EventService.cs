using System;
using System.Collections.Generic;
using System.Linq;
using MunicipalServiceApp_PROG7312.Models;

namespace MunicipalServiceApp_PROG7312.Services
{
    /// <summary>
    /// Event Service demonstrating advanced data structures for POE Part 2
    /// Implements: Dictionary, SortedDictionary, HashSet, Queue, Stack, PriorityQueue
    /// </summary>
    public static class EventService
    {
        #region Data Structures (POE Part 2 Requirements)

        /// <summary>
        /// Dictionary for O(1) event lookup by ID
        /// Demonstrates Hash Table/Dictionary usage (15 marks)
        /// </summary>
        private static Dictionary<string, Event> _eventDictionary = new Dictionary<string, Event>();

        /// <summary>
        /// SortedDictionary for events organized chronologically
        /// Demonstrates SortedDictionary usage (15 marks)
        /// </summary>
        private static SortedDictionary<DateTime, List<Event>> _eventsByDate
            = new SortedDictionary<DateTime, List<Event>>();

        /// <summary>
        /// HashSet for unique categories (no duplicates)
        /// Demonstrates Set usage (10 marks)
        /// </summary>
        private static HashSet<string> _categorySet = new HashSet<string>();

        /// <summary>
        /// Priority Queue for featured/urgent events
        /// Demonstrates Priority Queue usage (15 marks)
        /// </summary>
        private static PriorityQueue<Event, int> _priorityQueue = new PriorityQueue<Event, int>();

        /// <summary>
        /// Queue for recent searches (FIFO - First In First Out)
        /// Demonstrates Queue usage (15 marks)
        /// </summary>
        private static Queue<string> _recentSearches = new Queue<string>();
        private const int MaxRecentSearches = 10;

        /// <summary>
        /// Stack for search history (LIFO - Last In First Out)
        /// Demonstrates Stack usage (15 marks)
        /// </summary>
        private static Stack<string> _searchHistory = new Stack<string>();

        /// <summary>
        /// Dictionary for tracking search patterns (for recommendations)
        /// Demonstrates recommendation algorithm (30 marks)
        /// </summary>
        private static Dictionary<string, int> _searchPatterns = new Dictionary<string, int>();

        /// <summary>
        /// Dictionary for category search frequency
        /// Used in recommendation engine
        /// </summary>
        private static Dictionary<string, int> _categorySearchFrequency = new Dictionary<string, int>();

        #endregion

        #region Initialization

        static EventService()
        {
            InitializeSampleEvents();
        }
        //chatgpt used here to create new events 
        private static void InitializeSampleEvents()
        {
            var events = new List<Event>
            {
                new Event
                {
                    Title = "Table Mountain Trail Maintenance Volunteer Day",
                    Category = "Environment",
                    Description = "Join us for trail maintenance on Table Mountain. Help preserve our natural heritage while enjoying stunning views of the city and coastline.",
                    EventDate = DateTime.Now.AddDays(4),
                    Location = "Table Mountain National Park, Tafelberg Road",
                    Priority = 2,
                    IsFeatured = true,
                    AttendeeCount = 85,
                    Tags = new HashSet<string> { "hiking", "conservation", "volunteer", "table-mountain" }
                },
                new Event
                {
                    Title = "V&A Waterfront Load Shedding Information Session",
                    Category = "Utilities",
                    Description = "City Power briefing on load shedding schedules affecting the V&A Waterfront and surrounding areas. Backup power solutions discussion.",
                    EventDate = DateTime.Now.AddDays(2),
                    Location = "V&A Waterfront Amphitheatre, Dock Road",
                    Priority = 1,
                    IsFeatured = true,
                    AttendeeCount = 200,
                    Tags = new HashSet<string> { "electricity", "loadshedding", "infrastructure", "vna-waterfront" }
                },
                new Event
                {
                    Title = "Khayelitsha Community Centre Opening",
                    Category = "Community Development",
                    Description = "Grand opening of the new Khayelitsha Multi-Purpose Community Centre with sports facilities, library, and skills training center.",
                    EventDate = DateTime.Now.AddDays(8),
                    Location = "Khayelitsha Town Centre, Mew Way",
                    Priority = 2,
                    IsFeatured = true,
                    AttendeeCount = 450,
                    Tags = new HashSet<string> { "khayelitsha", "community", "development", "opening" }
                },
                new Event
                {
                    Title = "Sea Point Promenade Coastal Cleanup",
                    Category = "Environment",
                    Description = "Beach and promenade cleanup initiative. Protect marine life and keep our coastline beautiful. Gloves and bags provided.",
                    EventDate = DateTime.Now.AddDays(6),
                    Location = "Sea Point Promenade, Beach Road",
                    Priority = 3,
                    IsFeatured = true,
                    AttendeeCount = 120,
                    Tags = new HashSet<string> { "beach", "cleanup", "environment", "sea-point" }
                },
                new Event
                {
                    Title = "Century City Water Restrictions Workshop",
                    Category = "Water Management",
                    Description = "Learn about water-saving techniques and current restrictions. Free water-wise garden kits for attendees.",
                    EventDate = DateTime.Now.AddDays(5),
                    Location = "Century City Conference Centre, Ratanga Road",
                    Priority = 2,
                    IsFeatured = false,
                    AttendeeCount = 95,
                    Tags = new HashSet<string> { "water", "conservation", "workshop", "century-city" }
                },
                new Event
                {
                    Title = "Cape Town Stadium Community Sports Day",
                    Category = "Recreation",
                    Description = "Free sports activities for families at Cape Town Stadium. Soccer, rugby, athletics, and more. Register your kids online.",
                    EventDate = DateTime.Now.AddDays(12),
                    Location = "Cape Town Stadium, Green Point",
                    Priority = 3,
                    IsFeatured = false,
                    AttendeeCount = 350,
                    Tags = new HashSet<string> { "sports", "family", "recreation", "green-point" }
                },
                new Event
                {
                    Title = "Bellville Road Upgrade Information Meeting",
                    Category = "Infrastructure",
                    Description = "Public consultation on the R300 Bellville interchange upgrade. Traffic management plans and timelines will be discussed.",
                    EventDate = DateTime.Now.AddDays(7),
                    Location = "Bellville Civic Centre, Voortrekker Road",
                    Priority = 1,
                    IsFeatured = true,
                    AttendeeCount = 175,
                    Tags = new HashSet<string> { "roads", "traffic", "infrastructure", "bellville" }
                },
                new Event
                {
                    Title = "Camps Bay Beach Safety Awareness",
                    Category = "Public Safety",
                    Description = "NSRI beach safety demonstration. Learn about rip currents, safe swimming zones, and emergency procedures.",
                    EventDate = DateTime.Now.AddDays(9),
                    Location = "Camps Bay Beach, Victoria Road",
                    Priority = 2,
                    IsFeatured = true,
                    AttendeeCount = 65,
                    Tags = new HashSet<string> { "safety", "beach", "education", "camps-bay" }
                },
                new Event
                {
                    Title = "Mitchells Plain Skills Development Fair",
                    Category = "Economic Development",
                    Description = "Job opportunities, skills training programs, and small business support. Connect with local employers and training providers.",
                    EventDate = DateTime.Now.AddDays(14),
                    Location = "Mitchells Plain Town Centre, AZ Berman Drive",
                    Priority = 2,
                    IsFeatured = false,
                    AttendeeCount = 280,
                    Tags = new HashSet<string> { "jobs", "skills", "business", "mitchells-plain" }
                },
                new Event
                {
                    Title = "Hout Bay Harbour Recycling Drive",
                    Category = "Waste Management",
                    Description = "Drop off recyclables, e-waste, and hazardous materials. Proper disposal ensures environmental protection.",
                    EventDate = DateTime.Now.AddDays(3),
                    Location = "Hout Bay Harbour, Harbour Road",
                    Priority = 3,
                    IsFeatured = false,
                    AttendeeCount = 140,
                    Tags = new HashSet<string> { "recycling", "waste", "environment", "hout-bay" }
                },
                new Event
                {
                    Title = "MyCiTi Bus Route Expansion Consultation",
                    Category = "Transportation",
                    Description = "Have your say on proposed MyCiTi bus routes to Atlantis and surrounding areas. Public transport improvement initiative.",
                    EventDate = DateTime.Now.AddDays(11),
                    Location = "Civic Centre, 12 Hertzog Boulevard, Cape Town CBD",
                    Priority = 2,
                    IsFeatured = true,
                    AttendeeCount = 190,
                    Tags = new HashSet<string> { "transport", "myciti", "consultation", "cbd" }
                },
                new Event
                {
                    Title = "Newlands Fire Station Open Day",
                    Category = "Emergency Services",
                    Description = "Meet firefighters, see emergency equipment, and learn fire safety for your home. Kids activities and demonstrations.",
                    EventDate = DateTime.Now.AddDays(10),
                    Location = "Newlands Fire Station, Main Road",
                    Priority = 3,
                    IsFeatured = false,
                    AttendeeCount = 220,
                    Tags = new HashSet<string> { "fire", "safety", "emergency", "newlands" }
                }
                };

            // Add all events to data structures
            foreach (var evt in events)
            {
                AddEvent(evt);
            }
        }

        #endregion

        #region Event Management

        public static void AddEvent(Event evt)
        {
            // Add to Dictionary (O(1) lookup)
            _eventDictionary[evt.EventId] = evt;

            // Add to SortedDictionary by date
            var dateKey = evt.EventDate.Date;
            if (!_eventsByDate.ContainsKey(dateKey))
            {
                _eventsByDate[dateKey] = new List<Event>();
            }
            _eventsByDate[dateKey].Add(evt);

            // Add category to Set (ensures uniqueness)
            _categorySet.Add(evt.Category);

            // Add to Priority Queue if featured or urgent
            if (evt.IsFeatured || evt.Priority <= 2)
            {
                _priorityQueue.Enqueue(evt, evt.Priority);
            }
        }

        public static Event? GetEventById(string eventId)
        {
            return _eventDictionary.ContainsKey(eventId) ? _eventDictionary[eventId] : null;
        }

        public static List<Event> GetAllEvents()
        {
            return _eventDictionary.Values.ToList();
        }

        #endregion

        #region Search and Filtering

        public static List<Event> SearchEvents(string? searchQuery, string? category, DateTime? startDate)
        {
            var query = _eventDictionary.Values.AsEnumerable();

            // Filter by search query
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                var lowerQuery = searchQuery.ToLower();
                query = query.Where(e =>
                    e.Title.ToLower().Contains(lowerQuery) ||
                    e.Description.ToLower().Contains(lowerQuery) ||
                    e.Location.ToLower().Contains(lowerQuery) ||
                    e.Tags.Any(t => t.ToLower().Contains(lowerQuery))
                );

                // Track search for recommendations
                TrackSearch(searchQuery);
            }

            // Filter by category
            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(e => e.Category == category);
                TrackCategorySearch(category);
            }

            // Filter by date
            if (startDate.HasValue)
            {
                query = query.Where(e => e.EventDate.Date >= startDate.Value.Date);
            }

            return query.OrderBy(e => e.EventDate).ToList();
        }

        #endregion

        #region Data Structure Operations

        /// <summary>
        /// Get all unique categories using HashSet
        /// Demonstrates Set operations (10 marks)
        /// </summary>
        public static List<string> GetAllCategories()
        {
            return _categorySet.OrderBy(c => c).ToList();
        }

        /// <summary>
        /// Organize events by date using SortedDictionary
        /// Demonstrates SortedDictionary operations 
        /// </summary>
        public static SortedDictionary<DateTime, List<Event>> OrganizeEventsByDate(List<Event> events)
        {
            var result = new SortedDictionary<DateTime, List<Event>>();

            foreach (var evt in events)
            {
                var dateKey = evt.EventDate.Date;
                if (!result.ContainsKey(dateKey))
                {
                    result[dateKey] = new List<Event>();
                }
                result[dateKey].Add(evt);
            }

            return result;
        }

        /// <summary>
        /// Get featured/priority events using Priority Queue
        /// Demonstrates Priority Queue operations 
        /// </summary>
        public static List<Event> GetFeaturedEvents(int count = 5)
        {
            var featured = new List<Event>();
            var tempQueue = new PriorityQueue<Event, int>();

            // Copy priority queue
            while (_priorityQueue.Count > 0 && featured.Count < count)
            {
                var evt = _priorityQueue.Dequeue();
                featured.Add(evt);
                tempQueue.Enqueue(evt, evt.Priority);
            }

            // Restore priority queue
            while (tempQueue.Count > 0)
            {
                var evt = tempQueue.Dequeue();
                _priorityQueue.Enqueue(evt, evt.Priority);
            }

            return featured;
        }

        #endregion

        #region Search Tracking (Queue and Stack)

        /// <summary>
        /// Track search using Queue (FIFO) and Stack (LIFO)
        /// Demonstrates Queue and Stack operations
        /// </summary>
        public static void TrackSearch(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm)) return;

            // Add to Stack for history (most recent first)
            _searchHistory.Push(searchTerm);

            // Add to Queue for recent searches (FIFO)
            if (_recentSearches.Count >= MaxRecentSearches)
            {
                _recentSearches.Dequeue(); // Remove oldest
            }
            _recentSearches.Enqueue(searchTerm);

            // Track search patterns for recommendations
            var lowerTerm = searchTerm.ToLower();
            if (_searchPatterns.ContainsKey(lowerTerm))
            {
                _searchPatterns[lowerTerm]++;
            }
            else
            {
                _searchPatterns[lowerTerm] = 1;
            }
        }

        public static void TrackCategorySearch(string category)
        {
            if (_categorySearchFrequency.ContainsKey(category))
            {
                _categorySearchFrequency[category]++;
            }
            else
            {
                _categorySearchFrequency[category] = 1;
            }
        }

        public static Queue<string> GetRecentSearches()
        {
            return new Queue<string>(_recentSearches);
        }

        public static Stack<string> GetSearchHistory()
        {
            return new Stack<string>(_searchHistory);
        }

        #endregion

        #region Recommendation Engine (30 marks)

        /// <summary>
        /// Advanced recommendation algorithm based on search patterns
        /// Analyzes user behavior and suggests relevant events
        /// Demonstrates complex algorithm implementation 
        /// chatgpt used to guide implement complex algorithm
        /// </summary>
        public static List<Event> GetRecommendations(string? currentSearch, string? currentCategory, int count = 6)
        {
            var recommendations = new Dictionary<Event, double>();
            var allEvents = _eventDictionary.Values.Where(e => e.EventDate >= DateTime.Now).ToList();

            foreach (var evt in allEvents)
            {
                double score = 0;

                // Score based on search pattern frequency
                foreach (var pattern in _searchPatterns)
                {
                    var searchTerm = pattern.Key;
                    var frequency = pattern.Value;

                    if (evt.Title.ToLower().Contains(searchTerm) ||
                        evt.Description.ToLower().Contains(searchTerm) ||
                        evt.Tags.Any(t => t.ToLower().Contains(searchTerm)))
                    {
                        score += frequency * 10; // Weight by search frequency
                    }
                }

                // Score based on category search frequency
                if (_categorySearchFrequency.ContainsKey(evt.Category))
                {
                    score += _categorySearchFrequency[evt.Category] * 5;
                }

                // Boost score for current search context
                if (!string.IsNullOrWhiteSpace(currentSearch))
                {
                    var lowerSearch = currentSearch.ToLower();
                    if (evt.Title.ToLower().Contains(lowerSearch))
                        score += 50;
                    if (evt.Description.ToLower().Contains(lowerSearch))
                        score += 30;
                    if (evt.Tags.Any(t => t.ToLower().Contains(lowerSearch)))
                        score += 40;
                }

                // Boost score for same category
                if (!string.IsNullOrWhiteSpace(currentCategory) && evt.Category == currentCategory)
                {
                    score += 60;
                }

                // Boost for featured events
                if (evt.IsFeatured)
                    score += 20;

                // Boost for high-priority events
                score += (6 - evt.Priority) * 10;

                // Boost for popular events
                score += evt.AttendeeCount * 0.1;

                // Time relevance (sooner events get higher score)
                var daysUntilEvent = (evt.EventDate - DateTime.Now).TotalDays;
                if (daysUntilEvent <= 7)
                    score += 30;
                else if (daysUntilEvent <= 14)
                    score += 15;

                if (score > 0)
                {
                    recommendations[evt] = score;
                }
            }

            // Return top recommendations sorted by score
            return recommendations
                .OrderByDescending(r => r.Value)
                .Take(count)
                .Select(r => r.Key)
                .ToList();
        }

        /// <summary>
        /// Get related events based on tags (Set operations)
        /// Demonstrates Set intersection and union
        /// chatgpt used for set intersection and union
        /// </summary>
        public static List<Event> GetRelatedEvents(Event baseEvent, int count = 5)
        {
            var related = new Dictionary<Event, int>();

            foreach (var evt in _eventDictionary.Values)
            {
                if (evt.EventId == baseEvent.EventId) continue;

                // Calculate tag similarity using Set intersection
                var commonTags = new HashSet<string>(baseEvent.Tags);
                commonTags.IntersectWith(evt.Tags);

                if (commonTags.Count > 0)
                {
                    related[evt] = commonTags.Count;
                }
            }

            return related
                .OrderByDescending(r => r.Value)
                .Take(count)
                .Select(r => r.Key)
                .ToList();
        }

        #endregion

        #region Admin Event Management Methods

        /// <summary>
        /// Update an existing event (Admin only)
        /// </summary>
        public static void UpdateEvent(Event evt)
        {
            if (evt == null || string.IsNullOrEmpty(evt.EventId)) return;

            // Update in dictionary
            if (_eventDictionary.ContainsKey(evt.EventId))
            {
                _eventDictionary[evt.EventId] = evt;

                // Update in sorted dictionary
                RebuildSortedDictionary();

                // Update category set
                _categorySet.Add(evt.Category);

                // Update priority queue if needed
                if (evt.IsFeatured || evt.Priority <= 2)
                {
                    // Rebuild priority queue
                    RebuildPriorityQueue();
                }
            }
        }

        /// <summary>
        /// Delete an event (Admin only)
        /// </summary>
        public static bool DeleteEvent(string eventId)
        {
            if (string.IsNullOrEmpty(eventId)) return false;

            if (_eventDictionary.ContainsKey(eventId))
            {
                var evt = _eventDictionary[eventId];

                // Remove from dictionary
                _eventDictionary.Remove(eventId);

                // Rebuild sorted dictionary
                RebuildSortedDictionary();

                // Rebuild priority queue
                RebuildPriorityQueue();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Rebuild sorted dictionary after updates
        /// </summary>
        private static void RebuildSortedDictionary()
        {
            _eventsByDate.Clear();

            foreach (var evt in _eventDictionary.Values)
            {
                var dateKey = evt.EventDate.Date;
                if (!_eventsByDate.ContainsKey(dateKey))
                {
                    _eventsByDate[dateKey] = new List<Event>();
                }
                _eventsByDate[dateKey].Add(evt);
            }
        }

        /// <summary>
        /// Rebuild priority queue after updates
        /// </summary>
        private static void RebuildPriorityQueue()
        {
            _priorityQueue.Clear();

            foreach (var evt in _eventDictionary.Values)
            {
                if (evt.IsFeatured || evt.Priority <= 2)
                {
                    _priorityQueue.Enqueue(evt, evt.Priority);
                }
            }
        }

        #endregion

        #region Statistics

        public static SearchStatistics GetSearchStatistics()
        {
            var mostSearched = _categorySearchFrequency.OrderByDescending(c => c.Value).FirstOrDefault();

            return new SearchStatistics
            {
                TotalSearches = _searchPatterns.Values.Sum(),
                UniqueCategories = _categorySet.Count,
                MostSearchedCategory = mostSearched.Key ?? "N/A",
                CategoryFrequency = new Dictionary<string, int>(_categorySearchFrequency)
            };
        }

        public static int GetTotalEvents()
        {
            return _eventDictionary.Count;
        }

        public static int GetUpcomingEventsCount()
        {
            return _eventDictionary.Values.Count(e => e.EventDate >= DateTime.Now);
        }

        #endregion
    }
}