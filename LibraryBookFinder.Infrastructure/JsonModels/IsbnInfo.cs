namespace LibraryBookFinder.Infrastructure.JsonModels
{
    using Newtonsoft.Json;

    [JsonObject]
    public class IsbnInfo
    {
        [JsonProperty("type")]
        public string Type
        {
            get;
            set;
        }

        [JsonProperty("identifier")]
        public string Identifier
        {
            get;
            set;
        }

        public override string ToString()
        {
            return $"{ this.Type }: { this.Identifier }";
        }
    }
}
