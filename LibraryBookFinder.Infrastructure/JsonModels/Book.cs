namespace LibraryBookFinder.Infrastructure.JsonModels
{
    using Newtonsoft.Json;

    [JsonObject]
    public class Book
    {
        [JsonProperty("volumeInfo")]
        public VolumeInfo VolumeInfo
        {
            get;
            set;
        }

        public string Title
        {
            get
            {
                return this.VolumeInfo?.Title;
            }
        }

        public string[] Authors
        {
            get
            {
                return this.VolumeInfo?.Authors;
            }
        }
    }
}
