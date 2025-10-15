using System.ComponentModel.DataAnnotations;

namespace MunicipalServiceApp_PROG7312.Models
{
    /// <summary>
    /// Login model for admin authentication
    /// </summary>
    public class AdminLoginViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        [Display(Name = "Username")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }

    /// <summary>
   
    /// For POE demo: Username: admin, Password: Admin@123
    /// </summary>
    public static class AdminCredentials
    {
        public const string Username = "admin";
        public const string Password = "Admin@123"; // In production: use hashed passwords!

        public static bool ValidateCredentials(string username, string password)
        {
            return username == Username && password == Password;
        }
    }

    /// <summary>
    /// View model for creating/editing events (Admin only)
    /// </summary>
    public class ManageEventViewModel
    {
        public string? EventId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Event date is required")]
        [Display(Name = "Event Date")]
        public DateTime EventDate { get; set; } = DateTime.Now.AddDays(1);

        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; } = string.Empty;

        [Range(1, 5, ErrorMessage = "Priority must be between 1 and 5")]
        public int Priority { get; set; } = 3;

        [Display(Name = "Featured Event")]
        public bool IsFeatured { get; set; }

        [Display(Name = "Expected Attendees")]
        public int AttendeeCount { get; set; }

        [Display(Name = "Tags (comma-separated)")]
        public string TagsString { get; set; } = string.Empty;

        public List<string> AvailableCategories { get; set; } = new List<string>();
    }

    /// <summary>
    /// View model for admin dashboard
    /// </summary>
    public class AdminDashboardViewModel
    {
        public List<Event> AllEvents { get; set; } = new List<Event>();
        public int TotalEvents { get; set; }
        public int UpcomingEvents { get; set; }
        public int FeaturedEvents { get; set; }
        public Dictionary<string, int> EventsByCategory { get; set; } = new Dictionary<string, int>();
        public List<Event> RecentEvents { get; set; } = new List<Event>();
    }
}