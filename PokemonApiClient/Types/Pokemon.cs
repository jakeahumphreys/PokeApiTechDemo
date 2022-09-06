using System.Collections.Generic;
using Newtonsoft.Json;

namespace PokemonApiClient.Types
{
    public class Pokemon
    {
        [JsonProperty("abilities")]
        public List<AbilityMeta> Abilities { get; set; }

        [JsonProperty("base_experience")]
        public int BaseExperience { get; set; }

        [JsonProperty("forms")]
        public List<PokemonForm> Forms { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("is_default")]
        public bool IsDefault { get; set; }

        [JsonProperty("location_area_encounters")]
        public string LocationAreaEncounters { get; set; }

        [JsonProperty("moves")]
        public List<MoveMeta> Moves { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }

        [JsonProperty("species")]
        public Species Species { get; set; }

        [JsonProperty("sprites")]
        public Sprite Sprites { get; set; }

        [JsonProperty("stats")]
        public List<StatMeta> Stats { get; set; }

        [JsonProperty("types")]
        public List<TypeMeta> Types { get; set; }

        [JsonProperty("weight")]
        public int Weight { get; set; }
    }
}