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
    }
}
