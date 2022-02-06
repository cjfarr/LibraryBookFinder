namespace LibraryBookFinder.Infrastructure.JsonModels
{
    using Newtonsoft.Json;
    using System;
    using System.Linq;

    [JsonObject]
    public class Book
    {
        public static Uri ThumbnailNotFoundUri
        {
            get;
            set;
        }

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
                if (!string.IsNullOrEmpty(this.VolumeInfo?.Images?.Thumbnail?.AbsolutePath))
                {
                    return this.VolumeInfo.Images.Thumbnail;
                }

                return ThumbnailNotFoundUri;
            }
        }

        public string Publisher
        {
            get
            {
                return string.IsNullOrEmpty(this.VolumeInfo?.Publisher) ? "Unknown" : this.VolumeInfo.Publisher;
            }
        }

        public string PublishDate
        {
            get
            {
                return string.IsNullOrEmpty(this.VolumeInfo?.PublishedDate) ? "Unknown" : this.VolumeInfo.PublishedDate;
            }
        }

        public string[] Isbn
        {
            get
            {
                if (this.VolumeInfo?.Isbn == null)
                {
                    return new string[] { "Unknown" };
                }

                return this.VolumeInfo.Isbn.Select(i => i.ToString()).ToArray();
            }
        }
    }
}
