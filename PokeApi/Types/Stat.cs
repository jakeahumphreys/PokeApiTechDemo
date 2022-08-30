using Newtonsoft.Json;

namespace PokeApiTechDemo.PokeApi.Types
{
    public class Stat
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}