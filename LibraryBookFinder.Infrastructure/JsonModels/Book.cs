namespace LibraryBookFinder.Infrastructure.JsonModels
{
    using Newtonsoft.Json;
    using System;

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

        public Uri ImageThumbnail
        {
            get
            {
                return this.VolumeInfo?.Images?.Thumbnail;
            }
        }
    }
}
