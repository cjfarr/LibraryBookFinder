namespace LibraryBookFinder.Infrastructure.Services
{
    using LibraryBookFinder.Infrastructure.Interfaces;
    using LibraryBookFinder.Infrastructure.JsonModels;
    using Newtonsoft.Json;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class GoogleBookService : IGoogleBookService
    {
        private readonly HttpClient httpClient;

        public GoogleBookService()
        {
            this.httpClient = new HttpClient();
        }

        public async Task<BookCollection> RequestBooks(string titleSearch, int paginationOffset, int paginationLenth = 10)
        {
            string uri = $"https://www.googleapis.com/books/v1/volumes?q={titleSearch}&startIndex={paginationOffset}&maxResults={paginationLenth}";
            BookCollection collection = null;

            Task<string> result = this.httpClient.GetStringAsync(uri);
            string json = await result.ConfigureAwait(false);

            using (JsonReader reader = new JsonTextReader(new StringReader(json)))
            {
                JsonSerializer serializer = JsonSerializer.Create();
                collection = serializer.Deserialize<BookCollection>(reader);
            }

            return collection;   
        }
    }
}
