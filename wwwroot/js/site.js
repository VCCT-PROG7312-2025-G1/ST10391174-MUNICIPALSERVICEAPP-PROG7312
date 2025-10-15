
document.addEventListener('DOMContentLoaded', function () {
    // Initialize application
    initializeApp();
});

function initializeApp() {
    // Check if we're on the Report Issues page
    if (window.location.pathname.includes('ReportIssue')) {
        initializeReportIssuesPage();
    }

    // Check if we're on the Issue Submitted page
    if (window.location.pathname.includes('IssueSubmitted')) {
        initializeIssueSubmittedPage();
    }

    // Initialize common functionality
    initializeCommonFeatures();
}

function initializeReportIssuesPage() {
    let formInteractionStarted = false;
    let progressTimeout;

    // Load system analytics
    loadSystemAnalytics();

    // Enhanced progress tracking with debounce
    const progressElements = document.querySelectorAll('#locationInput, #categorySelect, #descriptionInput');
    progressElements.forEach(element => {
        element.addEventListener('input', function () {
            if (!formInteractionStarted) {
                formInteractionStarted = true;
                const analyticsPanel = document.getElementById('analyticsPanel');
                if (analyticsPanel) {
                    analyticsPanel.style.display = 'block';
                }
            }

            clearTimeout(progressTimeout);
            progressTimeout = setTimeout(updateFormProgress, 300);
        });

        element.addEventListener('change', function () {
            if (!formInteractionStarted) {
                formInteractionStarted = true;
                const analyticsPanel = document.getElementById('analyticsPanel');
                if (analyticsPanel) {
                    analyticsPanel.style.display = 'block';
                }
            }

            clearTimeout(progressTimeout);
            progressTimeout = setTimeout(updateFormProgress, 300);
        });
    });

    // Character and word count for description
    const descriptionInput = document.getElementById('descriptionInput');
    if (descriptionInput) {
        descriptionInput.addEventListener('input', function () {
            updateCharacterCount(this);
            clearTimeout(progressTimeout);
            progressTimeout = setTimeout(updateFormProgress, 300);
        });
    }

    // File upload handling
    const mediaFile = document.getElementById('mediaFile');
    if (mediaFile) {
        mediaFile.addEventListener('change', function () {
            handleFileUpload(this);
            updateFormProgress();
        });
    }

    // Form submission
    const issueForm = document.getElementById('issueForm');
    if (issueForm) {
        issueForm.addEventListener('submit', function (e) {
            return validateAndSubmitForm(e);
        });
    }

    // Auto-save functionality
    implementAutoSave();

    // Smart category suggestions
    implementSmartCategorySuggestions();

    // Input animations
    initializeInputAnimations();

    // Initialize progress
    updateFormProgress();
}

function initializeIssueSubmittedPage() {
    // Simulate real-time status updates
    setTimeout(function () {
        showStatusUpdate("Under Review", 50, "Your issue is now being reviewed by our technical team.");
    }, 3000);

    setTimeout(function () {
        showStatusUpdate("Assigned to Department", 75, "Issue has been assigned to the relevant municipal department.");
    }, 6000);

    // Track status button handler
    const trackStatusBtn = document.getElementById('trackStatusBtn');
    if (trackStatusBtn) {
        trackStatusBtn.addEventListener('click', function () {
            alert('Status tracking feature will be implemented in Part 3 using advanced tree structures and graph algorithms for efficient status management.');
        });
    }
}

function initializeCommonFeatures() {
    // Disable features not implemented in Part 1
    disableUnimplementedFeatures();

    // Initialize help system
    initializeHelpSystem();
}

// Form Progress and Engagement Functions
function updateFormProgress() {
    const locationInput = document.getElementById('locationInput');
    const categorySelect = document.getElementById('categorySelect');
    const descriptionInput = document.getElementById('descriptionInput');

    const location = locationInput ? locationInput.value : '';
    const category = categorySelect ? categorySelect.value : '';
    const description = descriptionInput ? descriptionInput.value : '';

    // Make fetch request instead of jQuery AJAX
    const url = '/Home/GetProgressUpdate?' + new URLSearchParams({
        location: location,
        category: category,
        description: description
    });

    fetch(url)
        .then(response => response.json())
        .then(data => {
            animateProgressUpdate(data);
            updateSystemStats(data.stats);
        })
        .catch(error => {
            console.log('Progress update temporarily unavailable');
            // Fallback to client-side calculation
            const progress = calculateClientSideProgress(location, category, description);
            animateProgressUpdate({ progress: progress, message: getEngagementMessage(progress) });
        });
}

function animateProgressUpdate(data) {
    const progressBar = document.getElementById('progressBar');
    const progressBadge = document.getElementById('progressBadge');
    const engagementMessage = document.getElementById('engagementMessage');

    if (progressBar) {
        progressBar.style.width = data.progress + '%';
        updateProgressStyling(data.progress);
    }

    if (progressBadge) {
        progressBadge.textContent = data.progress + '% Complete';
    }

    if (engagementMessage) {
        engagementMessage.textContent = data.message;
    }
}

function updateProgressStyling(progress) {
    const progressBar = document.getElementById('progressBar');
    if (progressBar) {
        progressBar.classList.remove('bg-danger', 'bg-warning', 'bg-success');

        if (progress < 40) {
            progressBar.classList.add('bg-danger');
        } else if (progress < 80) {
            progressBar.classList.add('bg-warning');
        } else {
            progressBar.classList.add('bg-success');
        }
    }
}

function calculateClientSideProgress(location, category, description) {
    let completedFields = 0;
    const totalFields = 3;

    if (location && location.trim()) completedFields++;
    if (category && category.trim()) completedFields++;
    if (description && description.trim()) completedFields++;

    return Math.round((completedFields / totalFields) * 100);
}

function getEngagementMessage(progress) {
    if (progress === 0) {
        return "Your civic participation makes a difference! Start reporting to improve our community.";
    } else if (progress < 50) {
        return "Great start! Your input helps build a better municipality.";
    } else if (progress < 100) {
        return "Excellent progress! Your detailed reports help us serve you better.";
    } else {
        return "Perfect! Your complete report will receive prompt attention from our team.";
    }
}

// Character Count and Validation
function updateCharacterCount(textArea) {
    const text = textArea.value;
    const charCount = text.length;
    const wordCount = text.trim() ? text.trim().split(/\s+/).length : 0;

    const charCountElement = document.getElementById('charCount');
    const wordCountElement = document.getElementById('wordCount');

    if (charCountElement) charCountElement.textContent = charCount;
    if (wordCountElement) wordCountElement.textContent = wordCount;

    // Visual feedback for character limits
    if (charCountElement) {
        charCountElement.classList.remove('text-warning', 'text-danger', 'text-success');

        if (charCount > 450) {
            charCountElement.classList.add('text-danger');
        } else if (charCount > 400) {
            charCountElement.classList.add('text-warning');
        } else if (charCount >= 10) {
            charCountElement.classList.add('text-success');
        }
    }
}

// File Upload Handling
function handleFileUpload(fileInput) {
    const file = fileInput.files[0];
    if (!file) {
        clearFilePreview();
        return;
    }

    // Validate file size (5MB limit)
    const maxSize = 5 * 1024 * 1024; // 5MB in bytes
    if (file.size > maxSize) {
        alert('File size cannot exceed 5MB. Please select a smaller file.');
        fileInput.value = '';
        clearFilePreview();
        return;
    }

    // Validate file type
    const allowedTypes = [
        'image/jpeg', 'image/png', 'image/gif', 'image/bmp',
        'application/pdf', 'application/msword',
        'application/vnd.openxmlformats-officedocument.wordprocessingml.document',
        'text/plain'
    ];

    if (!allowedTypes.includes(file.type)) {
        alert('Please select a valid file type (JPG, PNG, GIF, BMP, PDF, DOC, DOCX, TXT).');
        fileInput.value = '';
        clearFilePreview();
        return;
    }

    // Show file preview
    showFilePreview(file);
}

function showFilePreview(file) {
    const fileName = file.name;
    const fileSize = (file.size / 1024 / 1024).toFixed(2);
    const fileType = file.type;

    let previewHtml = `
        <div class="alert alert-info fade-in mt-3">
            <div class="d-flex justify-content-between align-items-start">
                <div>
                    <i class="fas fa-file me-2"></i>
                    <strong>${fileName}</strong> 
                    <small class="text-muted">(${fileSize} MB)</small>
                </div>
                <button type="button" class="btn-close" onclick="clearFilePreview()"></button>
            </div>
        </div>
    `;

    const filePreview = document.getElementById('filePreview');
    const previewBtn = document.getElementById('previewBtn');

    // Add image preview for supported image types
    if (fileType.startsWith('image/')) {
        const reader = new FileReader();
        reader.onload = function (e) {
            const imagePreview = `<img src="${e.target.result}" class="img-thumbnail mt-2" style="max-height: 150px; max-width: 200px;">`;
            if (filePreview) {
                filePreview.innerHTML = previewHtml + imagePreview;
                filePreview.style.display = 'block';
            }
        };
        reader.readAsDataURL(file);
        if (previewBtn) previewBtn.style.display = 'block';
    } else {
        if (filePreview) {
            filePreview.innerHTML = previewHtml;
            filePreview.style.display = 'block';
        }
        if (previewBtn) previewBtn.style.display = 'none';
    }
}

function clearFilePreview() {
    const mediaFile = document.getElementById('mediaFile');
    const filePreview = document.getElementById('filePreview');
    const previewBtn = document.getElementById('previewBtn');

    if (mediaFile) mediaFile.value = '';
    if (filePreview) {
        filePreview.style.display = 'none';
        filePreview.innerHTML = '';
    }
    if (previewBtn) previewBtn.style.display = 'none';
    updateFormProgress();
}

// Form Validation and Submission
function validateAndSubmitForm(e) {
    const validationResults = performFormValidation();

    if (!validationResults.isValid) {
        e.preventDefault();
        showValidationErrors(validationResults.errors);
        return false;
    }

    // Show loading state
    showFormSubmissionLoading();

    return true; // Allow form to submit
}

function performFormValidation() {
    const errors = [];
    let isValid = true;

    // Validate location
    const locationInput = document.getElementById('locationInput');
    const location = locationInput ? locationInput.value.trim() : '';

    if (!location) {
        errors.push('Location is required');
        if (locationInput) {
            locationInput.classList.add('is-invalid');
            locationInput.classList.remove('is-valid');
        }
        isValid = false;
    } else if (location.length < 3) {
        errors.push('Location must be at least 3 characters long');
        if (locationInput) {
            locationInput.classList.add('is-invalid');
            locationInput.classList.remove('is-valid');
        }
        isValid = false;
    } else {
        if (locationInput) {
            locationInput.classList.remove('is-invalid');
            locationInput.classList.add('is-valid');
        }
    }

    // Validate category
    const categorySelect = document.getElementById('categorySelect');
    const category = categorySelect ? categorySelect.value : '';

    if (!category) {
        errors.push('Category is required');
        if (categorySelect) {
            categorySelect.classList.add('is-invalid');
            categorySelect.classList.remove('is-valid');
        }
        isValid = false;
    } else {
        if (categorySelect) {
            categorySelect.classList.remove('is-invalid');
            categorySelect.classList.add('is-valid');
        }
    }

    // Validate description
    const descriptionInput = document.getElementById('descriptionInput');
    const description = descriptionInput ? descriptionInput.value.trim() : '';

    if (!description) {
        errors.push('Description is required');
        if (descriptionInput) {
            descriptionInput.classList.add('is-invalid');
            descriptionInput.classList.remove('is-valid');
        }
        isValid = false;
    } else if (description.length < 10) {
        errors.push('Description must be at least 10 characters long');
        if (descriptionInput) {
            descriptionInput.classList.add('is-invalid');
            descriptionInput.classList.remove('is-valid');
        }
        isValid = false;
    } else if (description.length > 500) {
        errors.push('Description cannot exceed 500 characters');
        if (descriptionInput) {
            descriptionInput.classList.add('is-invalid');
            descriptionInput.classList.remove('is-valid');
        }
        isValid = false;
    } else {
        if (descriptionInput) {
            descriptionInput.classList.remove('is-invalid');
            descriptionInput.classList.add('is-valid');
        }
    }

    return { isValid: isValid, errors: errors };
}

function showValidationErrors(errors) {
    let errorMessage = 'Please correct the following errors:\n\n';
    errors.forEach(function (error, index) {
        errorMessage += (index + 1) + '. ' + error + '\n';
    });

    alert(errorMessage);
}

function showFormSubmissionLoading() {
    const submitBtn = document.getElementById('submitBtn');
    const spinner = document.getElementById('submitSpinner');

    if (submitBtn) submitBtn.disabled = true;
    if (spinner) spinner.style.display = 'inline-block';
}

// Auto-save functionality
function implementAutoSave() {
    let autoSaveTimeout;

    const inputs = document.querySelectorAll('#locationInput, #categorySelect, #descriptionInput');
    inputs.forEach(input => {
        input.addEventListener('input', function () {
            clearTimeout(autoSaveTimeout);
            autoSaveTimeout = setTimeout(function () {
                console.log('Auto-saving form data to prevent data loss...');
                saveFormDataToSession();
            }, 2000);
        });

        input.addEventListener('change', function () {
            clearTimeout(autoSaveTimeout);
            autoSaveTimeout = setTimeout(function () {
                console.log('Auto-saving form data to prevent data loss...');
                saveFormDataToSession();
            }, 2000);
        });
    });
}

function saveFormDataToSession() {
    const locationInput = document.getElementById('locationInput');
    const categorySelect = document.getElementById('categorySelect');
    const descriptionInput = document.getElementById('descriptionInput');

    const formData = {
        location: locationInput ? locationInput.value : '',
        category: categorySelect ? categorySelect.value : '',
        description: descriptionInput ? descriptionInput.value : '',
        timestamp: new Date().toISOString()
    };

    try {
        sessionStorage.setItem('municipalIssueFormData', JSON.stringify(formData));
    } catch (e) {
        console.log('Session storage not available');
    }
}

// Smart Category Suggestions
function implementSmartCategorySuggestions() {
    const locationInput = document.getElementById('locationInput');
    const categorySelect = document.getElementById('categorySelect');

    if (locationInput && categorySelect) {
        locationInput.addEventListener('input', function () {
            const location = this.value.toLowerCase();

            if (categorySelect.value) {
                return; // Don't override user's selection
            }

            // Smart suggestions based on keywords
            let suggestedCategory = '';

            if (location.includes('road') || location.includes('street') || location.includes('highway') || location.includes('pothole')) {
                suggestedCategory = 'Roads and Infrastructure';
            } else if (location.includes('water') || location.includes('tap') || location.includes('pipe') || location.includes('leak')) {
                suggestedCategory = 'Water and Sanitation';
            } else if (location.includes('electric') || location.includes('power') || location.includes('light') || location.includes('outage')) {
                suggestedCategory = 'Electricity';
            } else if (location.includes('trash') || location.includes('garbage') || location.includes('waste') || location.includes('bin')) {
                suggestedCategory = 'Waste Management';
            } else if (location.includes('park') || location.includes('playground') || location.includes('sports')) {
                suggestedCategory = 'Parks and Recreation';
            } else if (location.includes('crime') || location.includes('safety') || location.includes('security')) {
                suggestedCategory = 'Public Safety';
            }

            if (suggestedCategory) {
                categorySelect.value = suggestedCategory;
                categorySelect.classList.add('suggested-category');
                setTimeout(function () {
                    categorySelect.classList.remove('suggested-category');
                }, 2000);
                updateFormProgress();
            }
        });
    }
}

// Input Animations and UX Enhancements
function initializeInputAnimations() {
    const customControls = document.querySelectorAll('.form-control-custom');
    customControls.forEach(control => {
        control.addEventListener('focus', function () {
            const parent = this.parentElement;
            if (parent) parent.classList.add('focused');
        });

        control.addEventListener('blur', function () {
            const parent = this.parentElement;
            if (parent) parent.classList.remove('focused');
        });
    });
}

// System Analytics Functions
function loadSystemAnalytics() {
    fetch('/Home/GetSystemStats')
        .then(response => response.json())
        .then(data => {
            updateSystemStats(data);
        })
        .catch(error => {
            console.log('Analytics temporarily unavailable');
            const totalElement = document.getElementById('systemTotalIssues');
            const pendingElement = document.getElementById('systemPendingIssues');
            if (totalElement) totalElement.textContent = 'N/A';
            if (pendingElement) pendingElement.textContent = 'N/A';
        });
}

function updateSystemStats(stats) {
    if (stats) {
        const totalElement = document.getElementById('systemTotalIssues');
        const pendingElement = document.getElementById('systemPendingIssues');

        if (totalElement) totalElement.textContent = stats.TotalIssues || stats.totalIssues || 0;
        if (pendingElement) pendingElement.textContent = stats.PendingProcessing || stats.pendingProcessing || 0;
    }
}

// Issue Submitted Page Functions
function showStatusUpdate(newStatus, progress, message) {
    if (confirm(`Status Update: ${message}\n\nClick OK to acknowledge this update.`)) {
        const statusProgress = document.getElementById('statusProgress');
        if (statusProgress) {
            statusProgress.style.width = progress + '%';
        }
    }
}

// Utility Functions


function initializeHelpSystem() {
    // Initialize tooltips for form fields
    const descriptionInput = document.getElementById('descriptionInput');
    const categorySelect = document.getElementById('categorySelect');

    if (descriptionInput) {
        descriptionInput.setAttribute('title', 'Provide a detailed description of the issue to help our team understand and resolve it quickly');
    }

    if (categorySelect) {
        categorySelect.setAttribute('title', 'Select the category that best matches your issue');
    }
}

// Prevent double submission
let formSubmitted = false;
document.addEventListener('DOMContentLoaded', function () {
    const issueForm = document.getElementById('issueForm');
    if (issueForm) {
        issueForm.addEventListener('submit', function () {
            if (formSubmitted) {
                return false;
            }
            formSubmitted = true;
            return true;
        });
    }
});

// Performance monitoring
window.addEventListener('load', function () {
    const loadTime = performance.now();
    console.log(`Page loaded in ${Math.round(loadTime)}ms`);

    // Hide any loading overlays
    const loadingOverlay = document.getElementById('globalLoadingOverlay');
    if (loadingOverlay) {
        loadingOverlay.style.display = 'none';
    }
});