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
            this.inputValidation = new Regex(@"^\d+$");
        }

        public string LastJsonResponse
        {
            get;
            private set;
        }

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

            if (this.inputValidation.IsMatch(input))
            {
                return false;
            }

            return true;
        }
    }
}
