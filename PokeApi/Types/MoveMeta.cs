using Newtonsoft.Json;

namespace PokeApiTechDemo.PokeApi.Types
{
    public class MoveMeta
    {
        [JsonProperty("move")]
        public Move Move { get; set; }
    }
}