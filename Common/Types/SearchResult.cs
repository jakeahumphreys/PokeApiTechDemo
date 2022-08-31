using PokeApiTechDemo.PokeApi.Types;

namespace PokeApiTechDemo.Common.Types
{
    public sealed class SearchResult
    {
        public Pokemon Pokemon { get; set; }
        public string Source { get; set; }
    }
}