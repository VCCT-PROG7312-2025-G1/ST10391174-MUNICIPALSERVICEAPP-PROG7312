using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace MunicipalServiceApp_PROG7312.Models
{
    
    public class ReportIssueViewModel
    {
        [Required(ErrorMessage = "Location is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Location must be between 3 and 100 characters")]
        [Display(Name = "Location")]
        public string? Location { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public string? Category { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 500 characters")]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Attach Media File")]
        public IFormFile? MediaFile { get; set; }

        public List<string> Categories { get; set; } = new List<string>();
        public int ProgressPercentage { get; set; } = 0;
        public string EngagementMessage { get; set; } = "Your civic participation makes a difference! Start reporting to improve our community.";

      
        /// Character count for description field
   
        public int CharacterCount
        {
            get
            {
                return string.IsNullOrEmpty(Description) ? 0 : Description.Length;
            }
        }

        
        /// Word count for description field
       
        public int WordCount
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Description))
                    return 0;
                return Description.Trim().Split(new char[] { ' ', '\t', '\n', '\r' },
                    StringSplitOptions.RemoveEmptyEntries).Length;
            }
        }

       
        /// Indicates if file upload is valid
        
        public bool HasValidFile
        {
            get
            {
                return MediaFile != null && MediaFile.Length > 0;
            }
        }

        
        /// Custom validation for file upload
        
        public bool IsFileValid()
        {
            if (MediaFile == null || MediaFile.Length == 0)
                return true; // File is optional

            // Check file size (5MB limit as per POE requirements)
            if (MediaFile.Length > 5 * 1024 * 1024)
                return false;

            // Check file type
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".pdf", ".doc", ".docx", ".txt" };
            var fileName = MediaFile.FileName.ToLower();

            foreach (var extension in allowedExtensions)
            {
                if (fileName.EndsWith(extension))
                    return true;
            }

            return false;
        }

       
        /// Calculate form completion progress
        
        public int CalculateProgress()
        {
            int completedFields = 0;
            int totalRequiredFields = 3;

            if (!string.IsNullOrWhiteSpace(Location)) completedFields++;
            if (!string.IsNullOrWhiteSpace(Category)) completedFields++;
            if (!string.IsNullOrWhiteSpace(Description)) completedFields++;

            return (completedFields * 100) / totalRequiredFields;
        }

        
        /// Get engagement message based on progress
        
        public string GetEngagementMessage()
        {
            var progress = CalculateProgress();

            return progress switch
            {
                0 => "Your civic participation makes a difference! Start reporting to improve our community.",
                < 50 => "Great start! Your input helps build a better municipality.",
                < 100 => "Excellent progress! Your detailed reports help us serve you better.",
                _ => "Perfect! Your complete report will receive prompt attention from our team."
            };
        }
    }

    /// <summary>
    /// Data structure management class implementing required data structures for POE
    /// </summary>
    public static class IssueDataManager
    {
        // LinkedList for efficient insertion/deletion of issues
        private static LinkedList<Issue> issueLinkedList = new LinkedList<Issue>();

        // Stack for tracking recently submitted issues (LIFO)
        private static Stack<Issue> recentIssuesStack = new Stack<Issue>();

        // Queue for processing order management (FIFO)
        private static Queue<Issue> processingQueue = new Queue<Issue>();

        /// <summary>
        /// Add new issue using LinkedList structure
        /// </summary>
        public static void AddIssue(Issue issue)
        {
            issueLinkedList.AddLast(issue);
            recentIssuesStack.Push(issue);
            processingQueue.Enqueue(issue);
        }

        /// <summary>
        /// Get all issues by traversing LinkedList
        /// </summary>
        public static List<Issue> GetAllIssues()
        {
            var issues = new List<Issue>();
            var currentNode = issueLinkedList.First;

            while (currentNode != null)
            {
                issues.Add(currentNode.Value);
                currentNode = currentNode.Next;
            }

            return issues;
        }

        /// <summary>
        /// Find issue by ID using LinkedList traversal
        /// </summary>
        public static Issue? FindIssueById(string id)
        {
            var currentNode = issueLinkedList.First;

            while (currentNode != null)
            {
                if (currentNode.Value.IssueId == id)
                {
                    return currentNode.Value;
                }
                currentNode = currentNode.Next;
            }

            return null;
        }

        /// <summary>
        /// Get most recent issue from Stack
        /// </summary>
        public static Issue? GetMostRecentIssue()
        {
            return recentIssuesStack.Count > 0 ? recentIssuesStack.Peek() : null;
        }

        /// <summary>
        /// Get next issue for processing from Queue
        /// </summary>
        public static Issue? GetNextForProcessing()
        {
            return processingQueue.Count > 0 ? processingQueue.Peek() : null;
        }

        /// <summary>
        /// Get count of issues in specific category
        /// </summary>
        public static int GetCategoryCount(string category)
        {
            int count = 0;
            var currentNode = issueLinkedList.First;

            while (currentNode != null)
            {
                if (currentNode.Value.Category == category)
                {
                    count++;
                }
                currentNode = currentNode.Next;
            }

            return count;
        }

        /// <summary>
        /// Get total counts for analytics
        /// </summary>
        public static (int totalIssues, int pendingProcessing, int recentCount) GetSystemStats()
        {
            return (issueLinkedList.Count, processingQueue.Count, recentIssuesStack.Count);
        }
    }
}