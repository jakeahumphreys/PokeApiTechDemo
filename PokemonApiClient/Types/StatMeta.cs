using Newtonsoft.Json;

namespace PokemonApiClient.Types
{
    public class StatMeta
    {
        [JsonProperty("base_stat")]
        public int BaseStat { get; set; }

        [JsonProperty("effort")]
        public int Effort { get; set; }

        [JsonProperty("stat")]
        public Stat Stat { get; set; }
    }
}