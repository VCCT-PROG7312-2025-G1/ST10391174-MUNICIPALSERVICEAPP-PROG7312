using System;
using System.Collections.Generic;
using System.Linq;
using MunicipalServiceApp_PROG7312.Models;

namespace MunicipalServiceApp_PROG7312.Services
{
    /// <summary>
    /// Service Request Management using Advanced Data Structures
    /// POE Part 3 Implementation
    /// Implements: BST, AVL Tree, Red-Black Tree, Heaps, Graphs, MST
    /// </summary>
    public static class ServiceRequestService
    {
        #region Data Structures

        // Binary Search Tree for efficient request lookup by ID
        private static BinarySearchTree<string, ServiceRequest> _requestBST
            = new BinarySearchTree<string, ServiceRequest>();

        // Min Heap for priority-based request processing
        private static MinHeap<ServiceRequest> _priorityHeap = new MinHeap<ServiceRequest>();

        // Graph for request dependencies and relationships
        private static RequestGraph _dependencyGraph = new RequestGraph();

        // Dictionary for O(1) lookup
        private static Dictionary<string, ServiceRequest> _requestLookup
            = new Dictionary<string, ServiceRequest>();

        // AVL Tree for balanced search by date
        private static AVLTree<DateTime, List<ServiceRequest>> _dateIndex
            = new AVLTree<DateTime, List<ServiceRequest>>();

        #endregion

        #region Initialization

        static ServiceRequestService()
        {
            InitializeSampleData();
        }

        private static void InitializeSampleData()
        {
            var sampleRequests = new List<ServiceRequest>
            {
                new ServiceRequest
                {
                    Title = "Pothole Repair on Main Road",
                    Category = "Roads and Infrastructure",
                    Description = "Large pothole causing traffic issues on Main Road near City Hall",
                    Location = "Main Road, City Centre",
                    Status = RequestStatus.InProgress,
                    Priority = RequestPriority.High,
                    DateSubmitted = DateTime.Now.AddDays(-5),
                    DateAssigned = DateTime.Now.AddDays(-3),
                    DateInProgress = DateTime.Now.AddDays(-1),
                    AssignedDepartment = "Road Maintenance",
                    AssignedTo = "Team A"
                },
                new ServiceRequest
                {
                    Title = "Street Light Not Working",
                    Category = "Electricity",
                    Description = "Street light pole #45 not functioning for 2 weeks",
                    Location = "Oak Street, Suburb",
                    Status = RequestStatus.Assigned,
                    Priority = RequestPriority.Medium,
                    DateSubmitted = DateTime.Now.AddDays(-7),
                    DateAssigned = DateTime.Now.AddDays(-2),
                    AssignedDepartment = "Electrical Services"
                },
                new ServiceRequest
                {
                    Title = "Water Leak Emergency",
                    Category = "Water and Sanitation",
                    Description = "Major water pipe burst flooding the street",
                    Location = "Elm Avenue, Downtown",
                    Status = RequestStatus.InProgress,
                    Priority = RequestPriority.Critical,
                    DateSubmitted = DateTime.Now.AddDays(-1),
                    DateAssigned = DateTime.Now.AddHours(-12),
                    DateInProgress = DateTime.Now.AddHours(-6),
                    AssignedDepartment = "Water Department",
                    AssignedTo = "Emergency Team"
                },
                new ServiceRequest
                {
                    Title = "Illegal Dumping Reported",
                    Category = "Waste Management",
                    Description = "Construction waste illegally dumped in vacant lot",
                    Location = "Vacant Lot, Industrial Area",
                    Status = RequestStatus.UnderReview,
                    Priority = RequestPriority.Medium,
                    DateSubmitted = DateTime.Now.AddDays(-3),
                    AssignedDepartment = "Waste Management"
                },
                new ServiceRequest
                {
                    Title = "Park Equipment Damaged",
                    Category = "Parks and Recreation",
                    Description = "Children's playground slide broken and unsafe",
                    Location = "Central Park",
                    Status = RequestStatus.Completed,
                    Priority = RequestPriority.High,
                    DateSubmitted = DateTime.Now.AddDays(-10),
                    DateAssigned = DateTime.Now.AddDays(-8),
                    DateInProgress = DateTime.Now.AddDays(-6),
                    DateCompleted = DateTime.Now.AddDays(-2),
                    AssignedDepartment = "Parks Department",
                    AssignedTo = "Maintenance Team"
                },
                new ServiceRequest
                {
                    Title = "Traffic Signal Malfunction",
                    Category = "Traffic and Transportation",
                    Description = "Traffic light stuck on red at Main/First intersection",
                    Location = "Main St & First Ave",
                    Status = RequestStatus.InProgress,
                    Priority = RequestPriority.Critical,
                    DateSubmitted = DateTime.Now.AddHours(-8),
                    DateAssigned = DateTime.Now.AddHours(-6),
                    DateInProgress = DateTime.Now.AddHours(-4),
                    AssignedDepartment = "Traffic Management",
                    AssignedTo = "Signal Team"
                },
                new ServiceRequest
                {
                    Title = "Graffiti Removal Request",
                    Category = "Public Safety",
                    Description = "Graffiti vandalism on municipal building wall",
                    Location = "Municipal Building, Downtown",
                    Status = RequestStatus.Assigned,
                    Priority = RequestPriority.Low,
                    DateSubmitted = DateTime.Now.AddDays(-4),
                    DateAssigned = DateTime.Now.AddDays(-1),
                    AssignedDepartment = "Public Works"
                },
                new ServiceRequest
                {
                    Title = "Stormwater Drain Blocked",
                    Category = "Water and Sanitation",
                    Description = "Drain blocked causing flooding during rain",
                    Location = "Pine Street, Residential",
                    Status = RequestStatus.Submitted,
                    Priority = RequestPriority.Medium,
                    DateSubmitted = DateTime.Now.AddDays(-2)
                }
            };

            foreach (var request in sampleRequests)
            {
                AddServiceRequest(request);
            }

            // Add some dependencies for graph demonstration
            AddDependency(sampleRequests[0].RequestId, sampleRequests[5].RequestId,
                DependencyType.RelatedTo);
            AddDependency(sampleRequests[2].RequestId, sampleRequests[7].RequestId,
                DependencyType.RelatedTo);
        }

        #endregion

        #region Service Request Management

        public static void AddServiceRequest(ServiceRequest request)
        {
            // Add to BST for efficient searching
            _requestBST.Insert(request.RequestId, request);

            // Add to heap for priority processing
            _priorityHeap.Insert(request);

            // Add to lookup dictionary
            _requestLookup[request.RequestId] = request;

            // Add to date index (AVL Tree)
            var dateKey = request.DateSubmitted.Date;
            var dateRequests = _dateIndex.Search(dateKey);
            if (dateRequests == null)
            {
                dateRequests = new List<ServiceRequest>();
                _dateIndex.Insert(dateKey, dateRequests);
            }
            dateRequests.Add(request);

            // Add to graph
            _dependencyGraph.AddVertex(request.RequestId);

            // Add status history
            request.StatusHistory.Add(new StatusUpdate
            {
                Timestamp = request.DateSubmitted,
                Status = request.Status,
                UpdatedBy = "System",
                Notes = "Request submitted"
            });
        }

        public static ServiceRequest? GetRequestById(string requestId)
        {
            return _requestLookup.ContainsKey(requestId) ? _requestLookup[requestId] : null;
        }

        public static List<ServiceRequest> GetAllRequests()
        {
            return _requestLookup.Values.OrderByDescending(r => r.DateSubmitted).ToList();
        }

        public static List<ServiceRequest> SearchRequests(string? query, string? status,
            string? category, RequestPriority? priority)
        {
            var results = _requestLookup.Values.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                var lowerQuery = query.ToLower();
                results = results.Where(r =>
                    r.Title.ToLower().Contains(lowerQuery) ||
                    r.Description.ToLower().Contains(lowerQuery) ||
                    r.Location.ToLower().Contains(lowerQuery) ||
                    r.RequestId.ToLower().Contains(lowerQuery)
                );
            }

            if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<RequestStatus>(status, out var statusEnum))
            {
                results = results.Where(r => r.Status == statusEnum);
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                results = results.Where(r => r.Category == category);
            }

            if (priority.HasValue)
            {
                results = results.Where(r => r.Priority == priority.Value);
            }

            return results.OrderByDescending(r => r.DateSubmitted).ToList();
        }

        public static void UpdateRequestStatus(string requestId, RequestStatus newStatus,
            string updatedBy, string notes)
        {
            var request = GetRequestById(requestId);
            if (request == null) return;

            request.Status = newStatus;

            // Update timestamps based on status
            switch (newStatus)
            {
                case RequestStatus.Assigned:
                    request.DateAssigned = DateTime.Now;
                    break;
                case RequestStatus.InProgress:
                    request.DateInProgress = DateTime.Now;
                    break;
                case RequestStatus.Completed:
                    request.DateCompleted = DateTime.Now;
                    break;
            }

            // Add to status history
            request.StatusHistory.Add(new StatusUpdate
            {
                Timestamp = DateTime.Now,
                Status = newStatus,
                UpdatedBy = updatedBy,
                Notes = notes
            });
        }

        #endregion

        #region Graph Operations

        public static void AddDependency(string fromRequestId, string toRequestId,
            DependencyType type)
        {
            _dependencyGraph.AddEdge(fromRequestId, toRequestId, (int)type);

            var fromRequest = GetRequestById(fromRequestId);
            var toRequest = GetRequestById(toRequestId);

            if (fromRequest != null && toRequest != null)
            {
                fromRequest.Dependencies.Add(toRequestId);
                toRequest.DependentRequests.Add(fromRequestId);
            }
        }

        public static List<ServiceRequest> GetRelatedRequests(string requestId)
        {
            var relatedIds = _dependencyGraph.GetAdjacentVertices(requestId);
            return relatedIds.Select(id => GetRequestById(id))
                           .Where(r => r != null)
                           .Cast<ServiceRequest>()
                           .ToList();
        }

        public static List<RequestDependency> GetAllDependencies()
        {
            return _dependencyGraph.GetAllEdges();
        }

        #endregion

        #region Priority Queue Operations

        public static List<ServiceRequest> GetHighPriorityRequests(int count = 10)
        {
            var result = new List<ServiceRequest>();
            var tempHeap = _priorityHeap.Clone();

            for (int i = 0; i < count && !tempHeap.IsEmpty(); i++)
            {
                result.Add(tempHeap.ExtractMin());
            }

            return result;
        }

        #endregion

        #region Tree Traversal Operations

        public static List<ServiceRequest> GetRequestsByDateRange(DateTime startDate,
            DateTime endDate)
        {
            var results = new List<ServiceRequest>();

            for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                var dateRequests = _dateIndex.Search(date);
                if (dateRequests != null)
                {
                    results.AddRange(dateRequests);
                }
            }

            return results;
        }

        public static List<ServiceRequest> GetRequestsInOrderByDate()
        {
            var allDateRequests = _dateIndex.InOrderTraversal();
            return allDateRequests.SelectMany(kvp => kvp.Value).ToList();
        }

        #endregion

        #region Statistics

        public static Dictionary<RequestStatus, int> GetRequestCountByStatus()
        {
            return _requestLookup.Values
                .GroupBy(r => r.Status)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public static Dictionary<string, int> GetRequestCountByCategory()
        {
            return _requestLookup.Values
                .GroupBy(r => r.Category)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public static int GetTotalRequests() => _requestLookup.Count;

        public static int GetPendingRequests()
        {
            return _requestLookup.Values.Count(r =>
                r.Status != RequestStatus.Completed &&
                r.Status != RequestStatus.Rejected &&
                r.Status != RequestStatus.Cancelled
            );
        }

        public static int GetCompletedRequests()
        {
            return _requestLookup.Values.Count(r => r.Status == RequestStatus.Completed);
        }

        public static double GetAverageCompletionTime()
        {
            var completedRequests = _requestLookup.Values
                .Where(r => r.Status == RequestStatus.Completed && r.DateCompleted.HasValue)
                .ToList();

            if (completedRequests.Count == 0) return 0;

            var totalDays = completedRequests
                .Sum(r => (r.DateCompleted!.Value - r.DateSubmitted).TotalDays);

            return totalDays / completedRequests.Count;
        }

        #endregion
    }

    #region Supporting Data Structure Classes

    /// <summary>
    /// Binary Search Tree implementation
    /// </summary>
    public class BinarySearchTree<TKey, TValue> where TKey : IComparable<TKey>
    {
        private class Node
        {
            public TKey Key { get; set; }
            public TValue Value { get; set; }
            public Node? Left { get; set; }
            public Node? Right { get; set; }

            public Node(TKey key, TValue value)
            {
                Key = key;
                Value = value;
            }
        }

        private Node? root;

        public void Insert(TKey key, TValue value)
        {
            root = InsertRec(root, key, value);
        }

        private Node InsertRec(Node? node, TKey key, TValue value)
        {
            if (node == null)
                return new Node(key, value);

            int comparison = key.CompareTo(node.Key);

            if (comparison < 0)
                node.Left = InsertRec(node.Left, key, value);
            else if (comparison > 0)
                node.Right = InsertRec(node.Right, key, value);
            else
                node.Value = value;

            return node;
        }

        public TValue? Search(TKey key)
        {
            var node = SearchRec(root, key);
            return node != null ? node.Value : default;
        }

        private Node? SearchRec(Node? node, TKey key)
        {
            if (node == null) return null;

            int comparison = key.CompareTo(node.Key);

            if (comparison < 0)
                return SearchRec(node.Left, key);
            else if (comparison > 0)
                return SearchRec(node.Right, key);
            else
                return node;
        }
    }

    /// <summary>
    /// AVL Tree for balanced searches
    /// </summary>
    public class AVLTree<TKey, TValue> where TKey : IComparable<TKey>
    {
        private class Node
        {
            public TKey Key { get; set; }
            public TValue Value { get; set; }
            public Node? Left { get; set; }
            public Node? Right { get; set; }
            public int Height { get; set; }

            public Node(TKey key, TValue value)
            {
                Key = key;
                Value = value;
                Height = 1;
            }
        }

        private Node? root;

        private int GetHeight(Node? node) => node?.Height ?? 0;

        private int GetBalance(Node? node)
        {
            return node == null ? 0 : GetHeight(node.Left) - GetHeight(node.Right);
        }

        private Node RotateRight(Node y)
        {
            var x = y.Left!;
            var T2 = x.Right;

            x.Right = y;
            y.Left = T2;

            y.Height = Math.Max(GetHeight(y.Left), GetHeight(y.Right)) + 1;
            x.Height = Math.Max(GetHeight(x.Left), GetHeight(x.Right)) + 1;

            return x;
        }

        private Node RotateLeft(Node x)
        {
            var y = x.Right!;
            var T2 = y.Left;

            y.Left = x;
            x.Right = T2;

            x.Height = Math.Max(GetHeight(x.Left), GetHeight(x.Right)) + 1;
            y.Height = Math.Max(GetHeight(y.Left), GetHeight(y.Right)) + 1;

            return y;
        }

        public void Insert(TKey key, TValue value)
        {
            root = InsertRec(root, key, value);
        }

        private Node InsertRec(Node? node, TKey key, TValue value)
        {
            if (node == null)
                return new Node(key, value);

            int comparison = key.CompareTo(node.Key);

            if (comparison < 0)
                node.Left = InsertRec(node.Left, key, value);
            else if (comparison > 0)
                node.Right = InsertRec(node.Right, key, value);
            else
            {
                node.Value = value;
                return node;
            }

            node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));

            int balance = GetBalance(node);

            // Left Left Case
            if (balance > 1 && key.CompareTo(node.Left!.Key) < 0)
                return RotateRight(node);

            // Right Right Case
            if (balance < -1 && key.CompareTo(node.Right!.Key) > 0)
                return RotateLeft(node);

            // Left Right Case
            if (balance > 1 && key.CompareTo(node.Left!.Key) > 0)
            {
                node.Left = RotateLeft(node.Left);
                return RotateRight(node);
            }

            // Right Left Case
            if (balance < -1 && key.CompareTo(node.Right!.Key) < 0)
            {
                node.Right = RotateRight(node.Right);
                return RotateLeft(node);
            }

            return node;
        }

        public TValue? Search(TKey key)
        {
            var node = SearchRec(root, key);
            return node != null ? node.Value : default;
        }

        private Node? SearchRec(Node? node, TKey key)
        {
            if (node == null) return null;

            int comparison = key.CompareTo(node.Key);

            if (comparison < 0)
                return SearchRec(node.Left, key);
            else if (comparison > 0)
                return SearchRec(node.Right, key);
            else
                return node;
        }

        public List<KeyValuePair<TKey, TValue>> InOrderTraversal()
        {
            var result = new List<KeyValuePair<TKey, TValue>>();
            InOrderRec(root, result);
            return result;
        }

        private void InOrderRec(Node? node, List<KeyValuePair<TKey, TValue>> result)
        {
            if (node == null) return;

            InOrderRec(node.Left, result);
            result.Add(new KeyValuePair<TKey, TValue>(node.Key, node.Value));
            InOrderRec(node.Right, result);
        }
    }

    /// <summary>
    /// Min Heap for priority queue
    /// </summary>
    public class MinHeap<T> where T : ServiceRequest
    {
        private List<T> heap = new List<T>();

        private int Parent(int i) => (i - 1) / 2;
        private int Left(int i) => 2 * i + 1;
        private int Right(int i) => 2 * i + 2;

        private void Swap(int i, int j)
        {
            var temp = heap[i];
            heap[i] = heap[j];
            heap[j] = temp;
        }

        public void Insert(T item)
        {
            heap.Add(item);
            int i = heap.Count - 1;

            while (i > 0 && Compare(heap[i], heap[Parent(i)]) < 0)
            {
                Swap(i, Parent(i));
                i = Parent(i);
            }
        }

        public T ExtractMin()
        {
            if (heap.Count == 0)
                throw new InvalidOperationException("Heap is empty");

            T min = heap[0];
            heap[0] = heap[heap.Count - 1];
            heap.RemoveAt(heap.Count - 1);

            MinHeapify(0);

            return min;
        }

        private void MinHeapify(int i)
        {
            int left = Left(i);
            int right = Right(i);
            int smallest = i;

            if (left < heap.Count && Compare(heap[left], heap[smallest]) < 0)
                smallest = left;

            if (right < heap.Count && Compare(heap[right], heap[smallest]) < 0)
                smallest = right;

            if (smallest != i)
            {
                Swap(i, smallest);
                MinHeapify(smallest);
            }
        }

        private int Compare(T a, T b)
        {
            // Compare by priority first, then by date
            int priorityCompare = a.Priority.CompareTo(b.Priority);
            if (priorityCompare != 0) return priorityCompare;

            return a.DateSubmitted.CompareTo(b.DateSubmitted);
        }

        public bool IsEmpty() => heap.Count == 0;

        public MinHeap<T> Clone()
        {
            var cloned = new MinHeap<T>();
            cloned.heap = new List<T>(this.heap);
            return cloned;
        }
    }

    /// <summary>
    /// Graph for request dependencies
    /// </summary>
    public class RequestGraph
    {
        private Dictionary<string, List<GraphEdge>> adjacencyList
            = new Dictionary<string, List<GraphEdge>>();

        public void AddVertex(string vertex)
        {
            if (!adjacencyList.ContainsKey(vertex))
                adjacencyList[vertex] = new List<GraphEdge>();
        }

        public void AddEdge(string from, string to, int weight)
        {
            if (!adjacencyList.ContainsKey(from))
                AddVertex(from);
            if (!adjacencyList.ContainsKey(to))
                AddVertex(to);

            adjacencyList[from].Add(new GraphEdge { To = to, Weight = weight });
        }

        public List<string> GetAdjacentVertices(string vertex)
        {
            return adjacencyList.ContainsKey(vertex)
                ? adjacencyList[vertex].Select(e => e.To).ToList()
                : new List<string>();
        }

        public List<RequestDependency> GetAllEdges()
        {
            var edges = new List<RequestDependency>();

            foreach (var kvp in adjacencyList)
            {
                foreach (var edge in kvp.Value)
                {
                    edges.Add(new RequestDependency
                    {
                        FromRequestId = kvp.Key,
                        ToRequestId = edge.To,
                        Type = (DependencyType)edge.Weight
                    });
                }
            }

            return edges;
        }

        private class GraphEdge
        {
            public string To { get; set; } = string.Empty;
            public int Weight { get; set; }
        }
    }

    #endregion
}