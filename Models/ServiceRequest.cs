using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MunicipalServiceApp_PROG7312.Models
{
    /// <summary>
    /// Service Request model for Part 3 - POE
    /// Represents a citizen's service request with tracking capabilities
    /// </summary>
    public class ServiceRequest
    {
        public string RequestId { get; set; } = string.Empty;

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Location { get; set; } = string.Empty;

        public RequestStatus Status { get; set; }
        public RequestPriority Priority { get; set; }

        public DateTime DateSubmitted { get; set; }
        public DateTime? DateAssigned { get; set; }
        public DateTime? DateInProgress { get; set; }
        public DateTime? DateCompleted { get; set; }

        public string? AssignedDepartment { get; set; }
        public string? AssignedTo { get; set; }

        public List<StatusUpdate> StatusHistory { get; set; } = new List<StatusUpdate>();
        public List<string> RelatedRequests { get; set; } = new List<string>();

        public string CitizenId { get; set; } = string.Empty;
        public string AttachmentPath { get; set; } = string.Empty;

        // For dependency tracking (graph relationships)
        public List<string> Dependencies { get; set; } = new List<string>();
        public List<string> DependentRequests { get; set; } = new List<string>();

        public ServiceRequest()
        {
            RequestId = GenerateRequestId();
            DateSubmitted = DateTime.Now;
            Status = RequestStatus.Submitted;
            Priority = RequestPriority.Medium;
        }

        private static string GenerateRequestId()
        {
            return "SR" + DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(100, 999);
        }

        public double GetCompletionProgress()
        {
            return Status switch
            {
                RequestStatus.Submitted => 10,
                RequestStatus.UnderReview => 25,
                RequestStatus.Assigned => 40,
                RequestStatus.InProgress => 70,
                RequestStatus.Completed => 100,
                RequestStatus.Rejected => 0,
                _ => 0
            };
        }
    }

    /// <summary>
    /// Status update entry for tracking request history
    /// </summary>
    public class StatusUpdate
    {
        public DateTime Timestamp { get; set; }
        public RequestStatus Status { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }

    /// <summary>
    /// Service request status enumeration
    /// </summary>
    public enum RequestStatus
    {
        Submitted,
        UnderReview,
        Assigned,
        InProgress,
        OnHold,
        Completed,
        Rejected,
        Cancelled
    }

    /// <summary>
    /// Priority levels for service requests
    /// </summary>
    public enum RequestPriority
    {
        Critical = 1,
        High = 2,
        Medium = 3,
        Low = 4
    }

    /// <summary>
    /// View model for Service Request Status page
    /// </summary>
    public class ServiceRequestStatusViewModel
    {
        public string? SearchQuery { get; set; }
        public string? FilterStatus { get; set; }
        public string? FilterCategory { get; set; }
        public RequestPriority? FilterPriority { get; set; }

        public List<ServiceRequest> AllRequests { get; set; } = new List<ServiceRequest>();
        public List<ServiceRequest> FilteredRequests { get; set; } = new List<ServiceRequest>();

        // Statistics
        public int TotalRequests { get; set; }
        public int PendingRequests { get; set; }
        public int CompletedRequests { get; set; }
        public int InProgressRequests { get; set; }

        public Dictionary<RequestStatus, int> RequestsByStatus { get; set; }
            = new Dictionary<RequestStatus, int>();
        public Dictionary<string, int> RequestsByCategory { get; set; }
            = new Dictionary<string, int>();

        // For graph visualization
        public List<RequestDependency> DependencyGraph { get; set; }
            = new List<RequestDependency>();

        // Related requests using tree structure
        public Dictionary<string, List<ServiceRequest>> RelatedRequestsTree { get; set; }
            = new Dictionary<string, List<ServiceRequest>>();
    }

    /// <summary>
    /// Represents a dependency relationship between service requests
    /// </summary>
    public class RequestDependency
    {
        public string FromRequestId { get; set; } = string.Empty;
        public string ToRequestId { get; set; } = string.Empty;
        public DependencyType Type { get; set; }
    }

    public enum DependencyType
    {
        BlockedBy,
        RelatedTo,
        DuplicateOf,
        ChildOf
    }

    /// <summary>
    /// View model for tracking a specific request
    /// </summary>
    public class TrackRequestViewModel
    {
        public ServiceRequest? Request { get; set; }
        public List<ServiceRequest> RelatedRequests { get; set; } = new List<ServiceRequest>();
        public List<ServiceRequest> SimilarRequests { get; set; } = new List<ServiceRequest>();
        public List<StatusUpdate> Timeline { get; set; } = new List<StatusUpdate>();

        public double ProgressPercentage { get; set; }
        public int EstimatedDaysToCompletion { get; set; }
    }
}