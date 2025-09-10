using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MunicipalServiceApp_PROG7312.Models
{
    /// <summary>
    /// Core Issue model representing a municipal service request
    /// </summary>
    public class Issue
    {
        public string IssueId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Location is required")]
        [Display(Name = "Location")]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 500 characters")]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Attached File")]
        public string? AttachedFilePath { get; set; }

        public DateTime DateReported { get; set; }
        public string Status { get; set; } = string.Empty;
    }

}