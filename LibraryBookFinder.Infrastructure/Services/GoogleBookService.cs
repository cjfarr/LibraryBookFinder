namespace LibraryBookFinder.Infrastructure.Services
{
    using LibraryBookFinder.Infrastructure.Interfaces;
    using LibraryBookFinder.Infrastructure.JsonModels;
    using Newtonsoft.Json;
    using System.IO;
    using System.Net.Http;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public class GoogleBookService : IGoogleBookService
    {
        private readonly HttpClient httpClient;
        private readonly Regex inputValidation;

        public GoogleBookService()
        {
            this.httpClient = new HttpClient();
            this.inputValidation = new Regex(@"^\d+$");
        }

        public string LastJsonResponse
        {
            get;
            private set;
        }

        public async Task<BookCollection> RequestBooks(string titleSearch, int paginationOffset, int paginationLenth = 10)
        {
            string uri = $"https://www.googleapis.com/books/v1/volumes?q={titleSearch}&startIndex={paginationOffset}&maxResults={paginationLenth}";
            BookCollection collection = null;

            Task<string> result = this.httpClient.GetStringAsync(uri);
            this.LastJsonResponse = await result.ConfigureAwait(false);

            using (JsonReader reader = new JsonTextReader(new StringReader(this.LastJsonResponse)))
            {
                JsonSerializer serializer = JsonSerializer.Create();
                collection = serializer.Deserialize<BookCollection>(reader);
            }

            return collection;   
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
