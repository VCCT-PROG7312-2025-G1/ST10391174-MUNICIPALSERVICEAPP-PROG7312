Municipal Services Application - PROG7312 POE Part 1
Project Overview
This is a C# ASP.NET Core web application developed for PROG7312 POE Part 1, implementing a Municipal Services Portal for South African municipalities. The application focuses on the "Report Issues" functionality using advanced data structures as specified in the POE requirements.
Student Information

Module: PROG7312 - Programming 3B / AAPD7112 - Advanced Application Development
Project Name: MunicipalServiceApp-PROG7312
POE Part: 1 of 3
Implementation Status: Part 1 Complete - Report Issues Feature

Features Implemented (Part 1)
Core Functionality

Report Issues Module: Fully functional issue reporting system
Data Structure Implementation: LinkedList, Stack, and Queue for efficient data management
File Upload System: Support for images and documents with validation (5MB limit)
User Engagement Strategy: Real-time progress tracking and encouraging feedback
Form Validation: Comprehensive client-side and server-side validation
Smart Category Suggestions: Auto-suggests categories based on location keywords

Data Structures Utilized
1. LinkedList

Purpose: Primary storage for all submitted issues
Implementation: LinkedList<Issue> for dynamic storage
Operations: Add, traverse, search by ID, category filtering
Benefits: Efficient insertion/deletion, dynamic memory allocation

2. Stack (LIFO)

Purpose: Track recently submitted issues
Implementation: Stack<Issue> for recent history
Operations: Push new issues, peek for most recent
Benefits: Quick access to latest submissions

3. Queue (FIFO)

Purpose: Manage processing order for municipal departments
Implementation: Queue<Issue> for fair processing
Operations: Enqueue new issues, peek next for processing
Benefits: Ensures first-come-first-served processing

Technical Specifications
Technology Stack

Framework: ASP.NET Core (.NET 6/7/8)
Language: C#
Frontend: HTML5, CSS3, Bootstrap 5, Vanilla JavaScript
Authentication: ASP.NET Core Identity (optional, not required for POE)
File Structure: Standard MVC pattern (Models, Views, Controllers)

Project Structure
MunicipalServiceApp-PROG7312/
├── Controllers/
│   └── HomeController.cs           # Main controller with data structure logic
├── Models/
│   ├── Issue.cs                    # Issue model and data structures
│   ├── ReportIssueViewModel.cs     # Form binding model
│   └── ErrorViewModel.cs           # Error handling model
├── Views/
│   ├── Home/
│   │   ├── Index.cshtml           # Main menu (3 options, 2 disabled)
│   │   ├── ReportIssue.cshtml     # Issue reporting form
│   │   └── IssueSubmitted.cshtml  # Success confirmation page
│   └── Shared/
│       └── _Layout.cshtml         # Master layout template
├── wwwroot/
│   ├── css/
│   │   └── site.css               # Custom styling
│   ├── js/
│   │   └── site.js                # Client-side functionality
│   └── uploads/                   # File upload directory
├── Data/
│   └── ApplicationDbContext.cs    # Entity Framework context (for Identity)
├── Program.cs                     # Application configuration
└── README.md                      # This documentation file
Installation and Setup
Prerequisites

Visual Studio 2022 or later
.NET 6/7/8 SDK
SQL Server (for Identity, optional)
IIS Express (included with Visual Studio)

Compilation and Running Instructions

Clone or Extract the Project
Extract the submitted files to your desired directory

Open in Visual Studio
Open Visual Studio
File → Open → Project/Solution
Navigate to the project folder and open MunicipalServiceApp-PROG7312.sln

Restore NuGet Packages
Right-click on the solution in Solution Explorer
Select "Restore NuGet Packages"

Configure Database (Optional - Identity Only)
Update-Database
(This is only for the Identity system, not required for POE functionality)

Build the Project
Build → Build Solution (Ctrl+Shift+B)
Ensure no compilation errors occur

Run the Application
Press F5 or click the "Start" button
The application will open in your default browser
Application URL: https://localhost:[port]/


Usage Instructions
Main Menu Navigation

Launch the application to access the main menu
Three options are presented:

Report Issues (Active - Part 1 implementation)
Local Events and Announcements (Disabled - Part 2)
Service Request Status (Disabled - Part 3)



Reporting an Issue

Access Report Form

Click "Report Issue Now" from the main menu
You'll be redirected to the issue reporting form


Fill Required Fields

Location: Enter specific location (minimum 3 characters, maximum 100)
Category: Select from dropdown (9 predefined categories)
Description: Detailed description (10-500 characters)


Optional Features

File Attachment: Upload images or documents (max 5MB)
Smart Suggestions: Categories auto-suggest based on location keywords
Progress Tracking: Real-time completion progress displayed


Form Submission
Future Development (Parts 2 & 3)
Part 2 - Local Events and Announcements

Hash Tables for event categorization
Sorted Dictionaries for date-based organization
Sets for unique event management
Advanced search and filtering functionality

Part 3 - Service Request Status

Binary Search Trees for efficient status lookup
Graph algorithms for dependency tracking
Heap structures for priority management
Advanced tree traversal algorithms

Click "Submit Report" to process the issue
Form validation prevents submission of incomplete data
Success page displays issue details and data structure analytics
