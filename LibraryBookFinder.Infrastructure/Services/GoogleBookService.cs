namespace LibraryBookFinder.Infrastructure.Services
{
    using LibraryBookFinder.Infrastructure.Exceptions;
    using LibraryBookFinder.Infrastructure.Interfaces;
    using LibraryBookFinder.Infrastructure.JsonModels;
    using Newtonsoft.Json;
    using System.IO;
    using System.Net.Http;
    using System.Net.NetworkInformation;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public class GoogleBookService : IGoogleBookService
    {
        private readonly HttpClient httpClient;
        private readonly Regex inputValidation;

        private string baseUri; 

        public GoogleBookService(string baseGoogleBookApiUrl)
        {
            this.baseUri = baseGoogleBookApiUrl;
            this.httpClient = new HttpClient();
            ////prevent numbers only input
            this.inputValidation = new Regex(@"^\d+$");
        }

        /// <summary>
        /// For debug purposes only.  Making a way to review what was sent
        /// in case something looks off.  Will be restricted with #if DEBUG
        /// processing directive.
        /// </summary>
        public string LastJsonResponse
        {
            get;
            private set;
        }

        /// <summary>
        /// Use for sanitization check ahead of BuildGetRequestUrl
        /// </summary>
        /// <param name="input">the input author or title</param>
        /// <returns>true/false</returns>
        public bool CheckIfInputIsValid(string input)
        {
            if (input?.Length >= 50)
            {
                return false;
            }

            if (string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            ////prevent numbers only input
            if (this.inputValidation.IsMatch(input))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Builds a url based on (title | author), and takes pagination parameters
        /// </summary>
        /// <param name="titleSearchCriteria">the title</param>
        /// <param name="authorSearchCriteria">the author</param>
        /// <param name="paginationOffset">pagination offset</param>
        /// <param name="paginationLength">pagination block length</param>
        /// <returns>the url that can be used with RequestBooks</returns>
        /// <exception cref="MissingSearchCriteriaException"></exception>
        public string BuildGetRequestUrl(string titleSearchCriteria, string authorSearchCriteria, int paginationOffset, int paginationLength = 10)
        {
            if (string.IsNullOrEmpty(titleSearchCriteria) && string.IsNullOrEmpty(authorSearchCriteria))
            {
                throw new MissingSearchCriteriaException();
            }

            string url = string.Empty;
            if (string.IsNullOrEmpty(authorSearchCriteria))
            {
                url = $"{this.baseUri}?q=+intitle:{titleSearchCriteria}&startIndex={paginationOffset}&maxResults={paginationLength}";
            }
            else if (string.IsNullOrEmpty(titleSearchCriteria))
            {
                url = $"{this.baseUri}?q=+inauthor:{authorSearchCriteria}&startIndex={paginationOffset}&maxResults={paginationLength}";
            }
            else
            {
                url = $"{this.baseUri}?q=+intitle:{titleSearchCriteria}+inauthor:{authorSearchCriteria}&startIndex={paginationOffset}&maxResults={paginationLength}";
            }

            return url;
        }

        /// <summary>
        /// Requests books from Google Books API 
        /// </summary>
        /// <param name="url">the GET request url (use BuildGetRequestUrl to get the url)</param>
        /// <returns>a BookCollection</returns>
        /// <exception cref="NoNetworkException"></exception>
        public async Task<BookCollection> RequestBooks(string url)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                throw new NoNetworkException();
            }

            BookCollection collection = null;

            Task<string> result = this.httpClient.GetStringAsync(url);
            this.LastJsonResponse = await result.ConfigureAwait(false);

            using (JsonReader reader = new JsonTextReader(new StringReader(this.LastJsonResponse)))
            {
                JsonSerializer serializer = JsonSerializer.Create();
                collection = serializer.Deserialize<BookCollection>(reader);
            }

            return collection;
        }
    }
}
