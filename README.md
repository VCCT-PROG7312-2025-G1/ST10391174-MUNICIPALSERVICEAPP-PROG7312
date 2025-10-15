# Municipal Services Application - PROG7312 POE Parts 1 & 2

## 📌 Project Overview
This is a C# ASP.NET Core web application developed for PROG7312 POE Parts 1 & 2, implementing a Municipal Services Portal for Cape Town, South Africa. The application now includes both "Report Issues" (Part 1) and "Local Events & Announcements" (Part 2) functionality using advanced data structures as specified in the POE requirements.

## 👨‍🎓 Student Information
- **Module:** PROG7312 - Programming 3B / AAPD7112 - Advanced Application Development
- **Project Name:** MunicipalServiceApp-PROG7312
- **POE Part:** Parts 1 & 2 of 3
- **Implementation Status:** 
  - ✅ Part 1 Complete - Report Issues Feature
  - ✅ Part 2 Complete - Local Events & Announcements Feature
  - ⏳ Part 3 Pending - Service Request Status

---

## 🚀 Features Implemented

### Part 1: Report Issues

#### Core Functionality
- **Report Issues Module:** Fully functional issue reporting system
- **Data Structure Implementation:** LinkedList, Stack, and Queue for efficient data management
- **File Upload System:** Support for images and documents with validation (5MB limit)
- **User Engagement Strategy:** Real-time progress tracking and encouraging feedback
- **Form Validation:** Comprehensive client-side and server-side validation
- **Smart Category Suggestions:** Auto-suggests categories based on location keywords

### Part 2: Local Events & Announcements 

#### Core Functionality
- **Cape Town Events Hub:** 10+ authentic Cape Town events with real locations
- **Advanced Search System:** Tabbed interface with quick search and advanced filters
- **Smart Recommendation Engine:** Algorithm-based event suggestions (30 marks)
- **Timeline View:** Unique chronological event display with circular date markers
- **Real-time Filtering:** Search by keywords, category, and date
- **Recent Search Tracking:** Queue-based search history (FIFO)

#### Cape Town Specific Features
- **Authentic Locations:** Table Mountain, V&A Waterfront, Khayelitsha, Sea Point, etc.
- **Local Issues:** Load shedding awareness, water restrictions, MyCiTi transport
- **Community Events:** Beach cleanups, skills development, heritage tours
- **Municipal Services:** Road upgrades, infrastructure consultations, public safety

---

## 📚 Data Structures Utilized

### Part 1 Data Structures

#### 1. LinkedList
- **Purpose:** Primary storage for all submitted issues
- **Implementation:** `LinkedList<Issue>` for dynamic storage
- **Operations:** Add, traverse, search by ID, category filtering
- **Benefits:** Efficient insertion/deletion, dynamic memory allocation

#### 2. Stack (LIFO)
- **Purpose:** Track recently submitted issues
- **Implementation:** `Stack<Issue>` for recent history
- **Operations:** Push new issues, peek for most recent
- **Benefits:** Quick access to latest submissions

#### 3. Queue (FIFO)
- **Purpose:** Manage processing order for municipal departments
- **Implementation:** `Queue<Issue>` for fair processing
- **Operations:** Enqueue new issues, peek next for processing
- **Benefits:** Ensures first-come-first-served processing

### Part 2 Data Structures

#### 1. Dictionary (Hash Table) 
- **Purpose:** O(1) event lookup by unique EventId
- **Implementation:** `Dictionary<string, Event>` for fast retrieval
- **Operations:** Add, retrieve, update, delete events
- **Benefits:** Constant time complexity for searches

#### 2. SortedDictionary 
- **Purpose:** Organize events chronologically by date
- **Implementation:** `SortedDictionary<DateTime, List<Event>>`
- **Operations:** Automatic sorting, range queries, date-based grouping
- **Benefits:** Always maintains sorted order, efficient date navigation

#### 3. HashSet (Set) 
- **Purpose:** Store unique categories and event tags
- **Implementation:** `HashSet<string>` for categories and tags
- **Operations:** Add unique items, intersection, union operations
- **Benefits:** Prevents duplicates, fast membership testing

#### 4. Queue (FIFO) 
- **Purpose:** Track recent searches (First-In-First-Out)
- **Implementation:** `Queue<string>` with max 10 searches
- **Operations:** Enqueue new searches, dequeue oldest when full
- **Benefits:** Maintains search history in chronological order

#### 5. Stack (LIFO) 
- **Purpose:** Complete search history (Last-In-First-Out)
- **Implementation:** `Stack<string>` for full history
- **Operations:** Push searches, pop for undo functionality
- **Benefits:** Access most recent searches first

#### 6. PriorityQueue 
- **Purpose:** Manage featured and high-priority events
- **Implementation:** `PriorityQueue<Event, int>` with priority levels 1-5
- **Operations:** Enqueue with priority, dequeue highest priority
- **Benefits:** Automatic priority-based ordering

### Part 2 Advanced Algorithm

#### Recommendation Engine 
- **Algorithm Type:** Weighted scoring with multiple factors
- **Scoring Factors:**
  - Search pattern frequency (user behavior analysis)
  - Category search history (preference tracking)
  - Current search context (relevance boosting)
  - Event features (featured status, priority level)
  - Event popularity (attendee count)
  - Time relevance (upcoming events weighted higher)
- **Data Structures Used:**
  - `Dictionary<string, int>` for search pattern tracking
  - `Dictionary<string, int>` for category frequency
  - Scoring algorithm with 6 weighted factors
- **Output:** Top 6 recommended events based on combined score

---

## 🛠️ Technical Specifications

### Technology Stack
- **Framework:** ASP.NET Core (.NET 6/7/8)
- **Language:** C#
- **Frontend:** HTML5, CSS3, Bootstrap 5, Custom CSS
- **JavaScript:** Vanilla JavaScript (no frameworks)
- **Authentication:** Session-based admin system
- **File Structure:** Standard MVC pattern (Models, Views, Controllers, Services)


---

## ⚙️ Installation and Setup

### Prerequisites
- Visual Studio 2022 or later
- .NET 6/7/8 SDK
- SQL Server (for Identity, optional)
- IIS Express (included with Visual Studio)

### Installation Steps

#### 1. Clone or Extract the Project
```bash
# Extract files to your desired directory
```

#### 2. Open in Visual Studio
```
File → Open → Project/Solution → Select MunicipalServiceApp-PROG7312.sln
```

#### 3. Restore NuGet Packages
```
Right-click solution → Restore NuGet Packages
```

#### 4. Configure Database (Optional - Identity Only)
```bash
Update-Database
# Only for Identity system, not required for POE functionality
```

#### 5. Build the Project
```
Build → Build Solution (Ctrl+Shift+B)
```

#### 6. Run the Application
```
Press F5 or click Start
App runs at: https://localhost:[port]/
```

---

## 🖥️ Usage Instructions

### Main Menu Navigation
1. **Report Issues** ✅ (Active - Part 1)
2. **Local Events & Announcements** ✅ (Active - Part 2)
3. **Service Request Status** ⏳ (Disabled - Part 3)
4. **Admin Portal** 🔐 (Admin login for event management)

### Part 1: Reporting an Issue

1. **Access Report Form**
   - Click "Report Issue Now" from main menu

2. **Fill Required Fields**
   - **Location** (3–100 chars)
   - **Category** (dropdown, 9 predefined)
   - **Description** (10–500 chars)

3. **Optional Features**
   - File Attachment (max 5MB)
   - Smart Suggestions (auto-category detection)
   - Progress Tracking (real-time feedback)

4. **Submit Form**
   - Validation ensures all required data
   - Success page displays issue details + data structure analytics

### Part 2: Browsing Local Events

#### Quick Search
1. **Access Events Hub**
   - Click "Local Events" from main menu

2. **Quick Search Tab**
   - Enter keywords in search bar
   - Click "Find" to search

3. **View Results**
   - Timeline view with chronological organization
   - Events grouped by date with circular markers

#### Advanced Filters
1. **Switch to Advanced Tab**
   - Click "Advanced Filters" tab

2. **Apply Filters**
   - **Keywords:** Enter search terms
   - **Category:** Select from dropdown (Environment, Utilities, etc.)
   - **Date:** Choose starting date

3. **View Filtered Results**
   - Events automatically filtered and sorted
   - Smart recommendations update based on search



#### Smart Recommendations
- Located in right sidebar
- Updates based on your search behavior
- Shows 4-6 personalized event suggestions
- Powered by multi-factor algorithm

#### Recent Searches
- Displays last 5 searches
- Click to quickly re-apply search
- Demonstrates Queue (FIFO) functionality

### Admin Features (Bonus)

#### Admin Login
- **URL:** `/Admin/Login`
- **Username:** `admin`
- **Password:** `Admin@123`

#### Admin Dashboard
- View all events in table format
- Statistics: Total events, upcoming, featured, categories
- Create, edit, and delete events
- Manage event priorities and featured status

#### Creating Events
1. Navigate to Admin Dashboard
2. Click "Create Event" button
3. Fill event details (title, category, description, etc.)
4. Set priority (1-5) and featured status
5. Add tags (comma-separated)
6. Submit to add to system

---

## 📂 Project Structure
```
MunicipalServiceApp-PROG7312/
├── Controllers/
│   ├── HomeController.cs          # Main controller (Report Issues + Events)
│   └── AdminController.cs         # Admin event management
├── Models/
│   ├── Issue.cs                   # Issue data model
│   ├── Event.cs                   # Event data model + ViewModels
│   ├── ReportIssueViewModel.cs    # Report form ViewModel
│   └── AdminModels.cs             # Admin login + management models
├── Services/
│   └── EventService.cs            # Event data structures + algorithms
├── Views/
│   ├── Home/
│   │   ├── Index.cshtml           # Main menu
│   │   ├── ReportIssue.cshtml     # Issue reporting form
│   │   ├── IssueSubmitted.cshtml  # Success confirmation
│   │   └── LocalEvents.cshtml     # Events timeline (Part 2)
│   ├── Admin/
│   │   ├── Login.cshtml           # Admin login
│   │   ├── Dashboard.cshtml       # Admin dashboard
│   │   ├── CreateEvent.cshtml     # Create event form
│   │   ├── EditEvent.cshtml       # Edit event form
│   │   └── DeleteEvent.cshtml     # Delete confirmation
│   └── Shared/
│       └── _Layout.cshtml         # Main layout with navigation
├── wwwroot/
│   ├── css/
│   │   └── site.css               # Custom styles
│   ├── js/
│   │   ├── site.js                # Global JavaScript
│   │   └── events.js              # Events page specific JS
│   └── uploads/                   # User uploaded files
└── Program.cs                     # App configuration + session setup
```

---



## 🔮 Future Development (Part 3)

### Part 3 - Service Request Status (Pending)
- **Binary Search Trees:** Status lookup and searching
- **AVL Trees:** Balanced tree for optimal performance
- **Red-Black Trees:** Self-balancing tree implementation
- **Graphs:** Dependency tracking between services
- **Graph Traversal:** BFS/DFS for status tracking
- **Heaps:** Priority queue for urgent requests
- **Minimum Spanning Tree:** Service optimization


**End of README - PROG7312 POE Parts 1 & 2**

*Developed as part of the Advanced Application Development curriculum at The Independent Institute of Education*
