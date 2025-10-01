namespace CaseStudyWebApi.Data.Nonpersistent
{
    /// <summary>
    /// Class for pagination metadata information
    /// </summary>
    public class PaginationMetadata
    {
        /// <summary>
        /// Total number of items in the collection
        /// </summary>
        public int TotalItemCount { get; set; }

        /// <summary>
        /// Current page size
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Current page number
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Total number of pages
        /// </summary>
        public int TotalPageCount { get; private set; }

        /// <summary>
        /// Indicates if there is a next page
        /// </summary>
        public bool HasNext => CurrentPage < TotalPageCount;

        /// <summary>
        /// Indicates if there is a previous page
        /// </summary>
        public bool HasPrevious => CurrentPage > 1;

        /// <summary>
        /// Constructor for pagination metadata
        /// </summary>
        /// <param name="totalItemCount">Total number of items in the collection</param>
        /// <param name="pageSize">Current page size</param>
        /// <param name="currentPage">Current page number</param>
        public PaginationMetadata(int totalItemCount, int pageSize, int currentPage)
        {
            TotalItemCount = totalItemCount;
            PageSize = pageSize;
            CurrentPage = currentPage;
            TotalPageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize);
        }
    }
}
