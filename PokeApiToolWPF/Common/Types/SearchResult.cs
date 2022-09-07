using PokeApiTool.Common.Types;
using PokemonApiClient.Types;

namespace PokeApiToolWPF.Common.Types
{
    public sealed class SearchResult
    {
        public Pokemon? Pokemon { get; set; }
        public string Source { get; set; }

        public SearchResult()
        {
            Source = ResultSourceType.UNKNOWN;
        }
    }
}