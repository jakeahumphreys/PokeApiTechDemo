using PokemonApiClient.Types;

namespace PokeApiTool.Common.Types
{
    public sealed class SearchResult
    {
        public Pokemon Pokemon { get; set; }
        public string Source { get; set; }
    }
}