using Newtonsoft.Json;

namespace PokeApiTechDemo.PokeApi.Types
{
    public class PokemonForm
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}