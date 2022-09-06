using Newtonsoft.Json;

namespace PokemonApiClient.Types
{
    public class Move
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}