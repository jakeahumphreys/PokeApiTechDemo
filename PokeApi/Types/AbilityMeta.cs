using Newtonsoft.Json;

namespace PokeApiTechDemo.PokeApi.Types
{
    public class AbilityMeta
    {
        [JsonProperty("ability")]
        public Ability Ability { get; set; }

        [JsonProperty("is_hidden")]
        public bool IsHidden { get; set; }

        [JsonProperty("slot")]
        public int Slot { get; set; }
    }
}