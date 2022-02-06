namespace LibraryBookFinder.Infrastructure.JsonModels
{
    using Newtonsoft.Json;

    [JsonObject]
    public class VolumeInfo
    {
        [JsonProperty("title")]
        public string Title
        {
            get;
            set;
        }

        [JsonProperty("authors")]
        public string[] Authors
        {
            get;
            set;
        }

        [JsonProperty("imageLinks")]
        public ImageLinks Images
        {
            get;
            set;
        }

        [JsonProperty("publisher")]
        public string Publisher
        {
            get;
            set;
        }

        [JsonProperty("publishedDate")]
        public string PublishedDate
        {
            get;
            set;
        }

        [JsonProperty("industryIdentifiers")]
        public IsbnInfo[] Isbn
        {
            get;
            set;
        }
    }
}
