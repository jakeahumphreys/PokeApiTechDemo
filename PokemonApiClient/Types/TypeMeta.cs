using Newtonsoft.Json;

namespace PokemonApiClient.Types
{
    public class TypeMeta
    {
        [JsonProperty("slot")]
        public int Slot { get; set; }
        [JsonProperty("type")]
        public Type Type { get; set; }
    }
}