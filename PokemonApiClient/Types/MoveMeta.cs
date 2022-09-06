using Newtonsoft.Json;

namespace PokemonApiClient.Types
{
    public class MoveMeta
    {
        [JsonProperty("move")]
        public Move Move { get; set; }
    }
}