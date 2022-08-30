using Newtonsoft.Json;

namespace PokeApiTechDemo.PokeApi.Types
{
    public class Sprite
    {
        [JsonProperty("back_default")]
        public string BackDefault { get; set; }

        [JsonProperty("back_female")]
        public object BackFemale { get; set; }

        [JsonProperty("back_shiny")]
        public string BackShiny { get; set; }

        [JsonProperty("back_shiny_female")]
        public object BackShinyFemale { get; set; }

        [JsonProperty("front_default")]
        public string FrontDefault { get; set; }

        [JsonProperty("front_female")]
        public object FrontFemale { get; set; }

        [JsonProperty("front_shiny")]
        public string FrontShiny { get; set; }

        [JsonProperty("front_shiny_female")]
        public object FrontShinyFemale { get; set; }
    }
}