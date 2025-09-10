**Municipal Services Application - PROG7312 POE Part 1
📌 Project Overview**

This is a C# ASP.NET Core web application developed for PROG7312 POE Part 1, implementing a Municipal Services Portal for South African municipalities.
The application focuses on the "Report Issues" functionality using advanced data structures as specified in the POE requirements.

👨‍🎓 **Student Information**

Module: PROG7312 - Programming 3B / AAPD7112 - Advanced Application Development

Project Name: MunicipalServiceApp-PROG7312

POE Part: 1 of 3

Implementation Status: ✅ Part 1 Complete - Report Issues Feature

🚀** Features Implemented (Part 1)
Core Functionality**

Report Issues Module: Fully functional issue reporting system

Data Structure Implementation: LinkedList, Stack, and Queue for efficient data management

File Upload System: Support for images and documents with validation (5MB limit)

User Engagement Strategy: Real-time progress tracking and encouraging feedback

Form Validation: Comprehensive client-side and server-side validation

Smart Category Suggestions: Auto-suggests categories based on location keywords

**📚 Data Structures Utilized
1. LinkedList**

Purpose: Primary storage for all submitted issues

Implementation: LinkedList<Issue> for dynamic storage

Operations: Add, traverse, search by ID, category filtering

Benefits: Efficient insertion/deletion, dynamic memory allocation

**2. Stack (LIFO)**

Purpose: Track recently submitted issues

Implementation: Stack<Issue> for recent history

Operations: Push new issues, peek for most recent

Benefits: Quick access to latest submissions

**3. Queue (FIFO)**

Purpose: Manage processing order for municipal departments

Implementation: Queue<Issue> for fair processing

Operations: Enqueue new issues, peek next for processing

Benefits: Ensures first-come-first-served processing

**🛠️ Technical Specifications**

Technology Stack

Framework: ASP.NET Core (.NET 6/7/8)

Language: C#

Frontend: HTML5, CSS3, Bootstrap 5, Vanilla JavaScript

Authentication: ASP.NET Core Identity (optional, not required for POE)

File Structure: Standard MVC pattern (Models, Views, Controllers)



**⚙️ Installation and Setup
Prerequisites**

Visual Studio 2022 or later

.NET 6/7/8 SDK

SQL Server (for Identity, optional)

IIS Express (included with Visual Studio)

Steps

Clone or Extract the Project

Extract files to your desired directory

Open in Visual Studio

File → Open → Project/Solution → Select MunicipalServiceApp-PROG7312.sln

Restore NuGet Packages

Right-click solution → Restore NuGet Packages

Configure Database (Optional - Identity Only)

Update-Database


(Only for Identity system, not required for POE functionality)

Build the Project

Build → Build Solution (Ctrl+Shift+B)

Run the Application

Press F5 or click Start

App runs at: https://localhost:[port]/

**🖥️ Usage Instructions
Main Menu Navigation**

Report Issues (Active - Part 1)

Local Events & Announcements (Disabled - Part 2)

Service Request Status (Disabled - Part 3)

Reporting an Issue

Access Report Form → "Report Issue Now"

Fill Required Fields

Location (3–100 chars)

Category (dropdown, 9 predefined)

Description (10–500 chars)

Optional Features

File Attachment (max 5MB)

Smart Suggestions (auto-category detection)

Progress Tracking

Submit Form

Validation ensures all required data

Success page displays issue details + data structure analytics

**🔮 Future Development (Parts 2 & 3)
Part 2 - Local Events & Announcements**

Hash Tables for categorization

Sorted Dictionaries for date-based organization

Sets for unique events

Advanced search + filtering

Part 3 - Service Request Status

Binary Search Trees for status lookup

Graph algorithms for dependency tracking

Heap structures for priority

Advanced tree traversal
