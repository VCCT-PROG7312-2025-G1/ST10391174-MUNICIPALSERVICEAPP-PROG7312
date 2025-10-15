// Municipal Services Application - Events Page JavaScript
// POE Part 2: Local Events and Announcements with Advanced Data Structures
//chatgpt was used to research different animations and transitions using javascript
//

var EventsPage = {
    searchTimeout: null,

    init: function () {
        if (typeof isLocalEventsPage === 'undefined' || !isLocalEventsPage) {
            return;
        }

        console.log('Initializing Events Page - POE Part 2');
        this.bindEvents();
        this.initializeRealTimeSearch();
        this.loadInitialRecommendations();
    },

    bindEvents: function () {
        var self = this;

        // Real-time search with debounce
        const searchInput = document.getElementById('searchQueryInput');
        if (searchInput) {
            searchInput.addEventListener('input', function () {
                clearTimeout(self.searchTimeout);
                self.searchTimeout = setTimeout(function () {
                    self.updateRecommendations();
                    self.trackUserSearch();
                }, 500);
            });
        }

        // Category filter change
        const categoryFilter = document.getElementById('categoryFilter');
        if (categoryFilter) {
            categoryFilter.addEventListener('change', function () {
                self.updateRecommendations();
                self.trackUserSearch();
            });
        }

        // Date filter change
        const dateFilter = document.getElementById('startDateFilter');
        if (dateFilter) {
            dateFilter.addEventListener('change', function () {
                self.trackUserSearch();
            });
        }

        // Form submission tracking
        const searchForm = document.getElementById('eventSearchForm');
        if (searchForm) {
            searchForm.addEventListener('submit', function () {
                self.trackUserSearch();
            });
        }
    },

    initializeRealTimeSearch: function () {
        console.log('Real-time search initialized with recommendation engine');
    },

    loadInitialRecommendations: function () {
        const searchQuery = document.getElementById('searchQueryInput');
        const category = document.getElementById('categoryFilter');

        if (searchQuery && searchQuery.value) {
            this.updateRecommendations();
        }
    },

    updateRecommendations: function () {
        const searchInput = document.getElementById('searchQueryInput');
        const categoryFilter = document.getElementById('categoryFilter');

        const searchQuery = searchInput ? searchInput.value : '';
        const category = categoryFilter ? categoryFilter.value : '';

        if (typeof searchRecommendationsUrl === 'undefined') {
            console.log('Recommendations URL not configured');
            return;
        }

        // Build query parameters
        const params = new URLSearchParams({
            searchQuery: searchQuery,
            category: category
        });

        fetch(`${searchRecommendationsUrl}?${params}`)
            .then(response => response.json())
            .then(data => {
                if (data.success && data.recommendations) {
                    this.displayRecommendations(data.recommendations);
                }
            })
            .catch(error => {
                console.log('Could not load recommendations:', error);
            });
    },

    displayRecommendations: function (recommendations) {
        const container = document.getElementById('recommendationsContainer');
        if (!container) return;

        if (recommendations.length === 0) {
            container.innerHTML = `
                <p class="text-muted text-center mb-0">
                    <i class="fas fa-info-circle me-1"></i>
                    No recommendations available for this search.
                </p>
            `;
            return;
        }

        let html = '';
        recommendations.forEach(function (evt) {
            const featuredIcon = evt.isFeatured ? '<i class="fas fa-star text-warning"></i>' : '';
            const priorityClass = evt.priority <= 2 ? 'border-warning' : '';

            html += `
                <div class="recommendation-item mb-3 p-2 border rounded ${priorityClass} fade-in">
                    <div class="d-flex justify-content-between align-items-start">
                        <div>
                            <h6 class="mb-1">${evt.title}</h6>
                            <small class="text-muted d-block">
                                <i class="fas fa-calendar me-1"></i>
                                ${this.formatDate(evt.eventDate)}
                            </small>
                            <small class="text-muted d-block">
                                <i class="fas fa-map-marker-alt me-1"></i>
                                ${evt.location}
                            </small>
                            <span class="badge bg-primary mt-1">${evt.category}</span>
                        </div>
                        ${featuredIcon}
                    </div>
                </div>
            `;
        }, this);

        container.innerHTML = html;

        // Add animation
        const items = container.querySelectorAll('.recommendation-item');
        items.forEach((item, index) => {
            setTimeout(() => {
                item.style.opacity = '0';
                item.style.transform = 'translateY(10px)';
                setTimeout(() => {
                    item.style.transition = 'all 0.3s ease';
                    item.style.opacity = '1';
                    item.style.transform = 'translateY(0)';
                }, 50);
            }, index * 50);
        });
    },

    trackUserSearch: function () {
        const searchInput = document.getElementById('searchQueryInput');
        const categoryFilter = document.getElementById('categoryFilter');

        const searchTerm = searchInput ? searchInput.value : '';
        const category = categoryFilter ? categoryFilter.value : '';

        if (!searchTerm && !category) return;

        if (typeof trackSearchUrl === 'undefined') {
            console.log('Track search URL not configured');
            return;
        }

        const data = {
            searchTerm: searchTerm,
            category: category
        };

        fetch(trackSearchUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data)
        })
            .then(response => response.json())
            .then(data => {
                console.log('Search tracked successfully');
            })
            .catch(error => {
                console.log('Could not track search:', error);
            });
    },

    formatDate: function (dateString) {
        const date = new Date(dateString);
        const options = {
            month: 'short',
            day: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
        };
        return date.toLocaleDateString('en-US', options);
    }
};

// Apply recent search
//chatgpt used to create this function 
function applyRecentSearch(searchTerm) {
    const searchInput = document.getElementById('searchQueryInput');
    if (searchInput) {
        searchInput.value = searchTerm;
        document.getElementById('eventSearchForm').submit();
    }
}

// View event details
function viewEventDetails(eventId) {
    alert(`Event Details for: ${eventId}\n\nThis feature will show full event details with related events using Set intersection operations.`);
}

// Show data structures information modal
function showDataStructuresInfo() {
    const modalHtml = `
        <div class="modal fade" id="dataStructuresModal" tabindex="-1" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header bg-primary text-white">
                        <h5 class="modal-title">
                            <i class="fas fa-database me-2"></i>Data Structures Implementation - POE Part 2
                        </h5>
                        <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"></button>
                    </div>
                    <div class="modal-body">
                        <h6 class="text-primary mb-3">Advanced Data Structures Used:</h6>
                        
                        <div class="accordion" id="dataStructuresAccordion">
                            <!-- Dictionary -->
                            <div class="accordion-item">
                                <h2 class="accordion-header">
                                    <button class="accordion-button" type="button" data-bs-toggle="collapse" data-bs-target="#dict">
                                        <i class="fas fa-book me-2 text-primary"></i>Dictionary (Hash Table)
                                    </button>
                                </h2>
                                <div id="dict" class="accordion-collapse collapse show">
                                    <div class="accordion-body">
                                        <strong>Purpose:</strong> Fast O(1) event lookup by ID<br>
                                        <strong>Usage:</strong> Stores all events with EventId as key<br>
                                        <strong>Benefit:</strong> Instant access to any event
                                    </div>
                                </div>
                            </div>

                            <!-- SortedDictionary -->
                            <div class="accordion-item">
                                <h2 class="accordion-header">
                                    <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#sortedDict">
                                        <i class="fas fa-sort-amount-up me-2 text-success"></i>SortedDictionary
                                    </button>
                                </h2>
                                <div id="sortedDict" class="accordion-collapse collapse">
                                    <div class="accordion-body">
                                        <strong>Purpose:</strong> Automatically organizes events by date<br>
                                        <strong>Usage:</strong> Groups events by date key<br>
                                        <strong>Benefit:</strong> Always maintains chronological order
                                    </div>
                                </div>
                            </div>

                            <!-- HashSet -->
                            <div class="accordion-item">
                                <h2 class="accordion-header">
                                    <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#hashSet">
                                        <i class="fas fa-list-ul me-2 text-warning"></i>HashSet (Set)
                                    </button>
                                </h2>
                                <div id="hashSet" class="accordion-collapse collapse">
                                    <div class="accordion-body">
                                        <strong>Purpose:</strong> Stores unique categories and tags<br>
                                        <strong>Usage:</strong> Prevents duplicate categories, enables tag matching<br>
                                        <strong>Benefit:</strong> Fast uniqueness checks and set operations
                                    </div>
                                </div>
                            </div>

                            <!-- Queue -->
                            <div class="accordion-item">
                                <h2 class="accordion-header">
                                    <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#queue">
                                        <i class="fas fa-arrow-right me-2 text-danger"></i>Queue (FIFO)
                                    </button>
                                </h2>
                                <div id="queue" class="accordion-collapse collapse">
                                    <div class="accordion-body">
                                        <strong>Purpose:</strong> Tracks recent searches in order<br>
                                        <strong>Usage:</strong> First In, First Out - oldest searches removed first<br>
                                        <strong>Benefit:</strong> Maintains search history chronologically
                                    </div>
                                </div>
                            </div>

                            <!-- Stack -->
                            <div class="accordion-item">
                                <h2 class="accordion-header">
                                    <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#stack">
                                        <i class="fas fa-layer-group me-2 text-info"></i>Stack (LIFO)
                                    </button>
                                </h2>
                                <div id="stack" class="accordion-collapse collapse">
                                    <div class="accordion-body">
                                        <strong>Purpose:</strong> Search history tracking<br>
                                        <strong>Usage:</strong> Last In, First Out - most recent first<br>
                                        <strong>Benefit:</strong> Quick access to latest searches
                                    </div>
                                </div>
                            </div>

                            <!-- PriorityQueue -->
                            <div class="accordion-item">
                                <h2 class="accordion-header">
                                    <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#priorityQueue">
                                        <i class="fas fa-exclamation-triangle me-2 text-secondary"></i>Priority Queue
                                    </button>
                                </h2>
                                <div id="priorityQueue" class="accordion-collapse collapse">
                                    <div class="accordion-body">
                                        <strong>Purpose:</strong> Manages featured/urgent events<br>
                                        <strong>Usage:</strong> Events ordered by priority level<br>
                                        <strong>Benefit:</strong> Important events always accessible first
                                    </div>
                                </div>
                            </div>

                            <!-- Recommendation Engine -->
                            <div class="accordion-item">
                                <h2 class="accordion-header">
                                    <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#recommendations">
                                        <i class="fas fa-magic me-2 text-success"></i>Recommendation Algorithm
                                    </button>
                                </h2>
                                <div id="recommendations" class="accordion-collapse collapse">
                                    <div class="accordion-body">
                                        <strong>Purpose:</strong> Suggests relevant events based on user behavior<br>
                                        <strong>Algorithm:</strong> Scores events based on:<br>
                                        <ul class="mb-0">
                                            <li>Search pattern frequency</li>
                                            <li>Category search history</li>
                                            <li>Event priority and popularity</li>
                                            <li>Time relevance</li>
                                            <li>Tag matching</li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="alert alert-success mt-4">
                            <h6><i class="fas fa-graduation-cap me-2"></i>POE Part 2 - Mark Distribution</h6>
                            <ul class="mb-0 small">
                                <li>Main Menu: 30 marks</li>
                                <li>Stacks, Queues, Priority Queues: 15 marks</li>
                                <li>Dictionaries, Sorted Dictionaries: 15 marks</li>
                                <li>Sets: 10 marks</li>
                                <li>Recommendation Feature: 30 marks</li>
                            </ul>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
    `;

    // Remove existing modal
    const existingModal = document.getElementById('dataStructuresModal');
    if (existingModal) {
        existingModal.remove();
    }

    // Add and show modal
    document.body.insertAdjacentHTML('beforeend', modalHtml);
    const modal = new bootstrap.Modal(document.getElementById('dataStructuresModal'));
    modal.show();

    // Cleanup on close
    document.getElementById('dataStructuresModal').addEventListener('hidden.bs.modal', function () {
        this.remove();
    });
}

// Initialize when DOM is ready
document.addEventListener('DOMContentLoaded', function () {
    EventsPage.init();
});

// Export for use in other scripts if needed
window.EventsPage = EventsPage;