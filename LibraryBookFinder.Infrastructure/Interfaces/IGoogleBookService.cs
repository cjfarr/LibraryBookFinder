namespace LibraryBookFinder.Infrastructure.Interfaces
{
    using LibraryBookFinder.Infrastructure.JsonModels;
    using System.Threading.Tasks;

    public interface IGoogleBookService
    {
        /// <summary>
        /// For debug purposes only.  Making a way to review what was sent
        /// in case something looks off.  Will be restricted with #if DEBUG
        /// processing directive.
        /// </summary>
        string LastJsonResponse 
        { 
            get; 
        }

        /// <summary>
        /// Use for sanitization check ahead of BuildGetRequestUrl
        /// </summary>
        /// <param name="input">the input author or title</param>
        /// <returns>true/false</returns>
        bool CheckIfInputIsValid(string input);

        /// <summary>
        /// Builds a url based on (title | author), and takes pagination parameters
        /// </summary>
        /// <param name="titleSearchCriteria">the title</param>
        /// <param name="authorSearchCriteria">the author</param>
        /// <param name="paginationOffset">pagination offset</param>
        /// <param name="paginationLength">pagination block length</param>
        /// <returns>the url that can be used with RequestBooks</returns>
        /// <exception cref="MissingSearchCriteriaException"></exception>
        string BuildGetRequestUrl(string titleSearchCriteria, string authorSearchCriteria, int paginationOffset, int paginationLength = 10);

        /// <summary>
        /// Requests books from Google Books API 
        /// </summary>
        /// <param name="url">the GET request url (use BuildGetRequestUrl to get the url)</param>
        /// <returns>a BookCollection</returns>
        /// <exception cref="NoNetworkException"></exception>
        Task<BookCollection> RequestBooks(string url);
    }
}
