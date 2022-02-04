namespace LibraryBookFinder.Infrastructure.JsonModels
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    [JsonObject]
    public class BookCollection
    {
        [JsonProperty("items")]
        public List<Book> Books 
        { 
            get; 
            set;
        }

        [JsonProperty("totalItems")]
        public int TotalItemsExpected
        {
            get;
            set;
        }
    }
}
