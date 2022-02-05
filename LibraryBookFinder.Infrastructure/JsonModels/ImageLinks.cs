namespace LibraryBookFinder.Infrastructure.JsonModels
{
    using Newtonsoft.Json;
    using System;

    [JsonObject]
    public class ImageLinks
    {
        [JsonProperty("smallThumbnail")]
        public Uri SmallThumbnail
        {
            get;
            set;
        }

        [JsonProperty("thumbnail")]
        public Uri Thumbnail
        {
            get;
            set;
        }
    }
}
