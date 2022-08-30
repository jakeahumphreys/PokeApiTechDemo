using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using PokeApiTechDemo.Cache;
using PokeApiTechDemo.Common.Types;
using PokeApiTechDemo.PokeApi;
using PokeApiTechDemo.PokeApi.Types;

namespace PokeApiTechDemo
{
    public partial class MainForm : Form
    {
        private readonly PokeApiClient _pokeApiClient;
        private readonly CacheService _cacheService;

        public MainForm()
        {
            InitializeComponent();
            _pokeApiClient = new PokeApiClient();
            _cacheService = new CacheService();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchForPokemon(txtSearch.Text);
        }
        
        private void lstHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            var searchText = lstHistory.SelectedItem.ToString();
            txtSearch.Text = searchText;
            lblStatus.Text = "Searching from history...";
            SearchForPokemon(searchText);
        }

        private void SearchForPokemon(string searchText)
        {
            Pokemon pokemon = null;
            string resultSource;
            
            var potentialCacheEntries = _cacheService.GetCacheEntriesForName(searchText);

            if (potentialCacheEntries.Count > 0)
            {
                var cacheEntry = potentialCacheEntries.First();

                if (cacheEntry.Time.AddMinutes(10) > DateTime.Now)
                {
                    pokemon = JsonConvert.DeserializeObject<Pokemon>(cacheEntry.Blob);
                    resultSource = ResultSourceType.RESULT_CACHE;
                }
                else
                {
                    pokemon = FetchPokemonFromApi(searchText);
                    resultSource = ResultSourceType.RESULT_API;
                }
            }
            else
            {
                pokemon = FetchPokemonFromApi(searchText);
                resultSource = ResultSourceType.RESULT_API;
            }

            PopulateFormFromResult(pokemon, resultSource);
        }

        private Pokemon FetchPokemonFromApi(string searchText)
        {
            var result = _pokeApiClient.GetPokemon(searchText);
            if (result.HasError)
                MessageBox.Show(result.Error.Message);

            var pokemon = result.Pokemon;
            _cacheService.CacheResult(pokemon.Name, JsonConvert.SerializeObject(pokemon));

            return pokemon;
        }

        private void PopulateFormFromResult(Pokemon pokemon, string resultSource)
        {
            lblStatus.Text = "Search complete!";
            
            lstHistory.Items.Add(pokemon.Name);
            //Set JSON Tab
            txtJson.Text = JsonConvert.SerializeObject(pokemon, Formatting.Indented);
            
            //Populate Tree View
            tvDetails.Nodes.Clear();

            var sourceNode = tvDetails.Nodes.Add("Result Source");
            sourceNode.Nodes.Add(resultSource);
                
            var detailsPrimaryNode = tvDetails.Nodes.Add(pokemon.Name);
            var detailsAbilitiesMetaNode = detailsPrimaryNode.Nodes.Add("Abilities");
            var detailsFormsNode = detailsPrimaryNode.Nodes.Add("Forms");
            var detailsMovesNode = detailsPrimaryNode.Nodes.Add("Moves");
            var detailsStatsNode = detailsPrimaryNode.Nodes.Add("Stats");

            foreach (var abilityMeta in pokemon.Abilities)
                detailsAbilitiesMetaNode.Nodes.Add(abilityMeta.Ability.Name);

            foreach (var form in pokemon.Forms)
                detailsFormsNode.Nodes.Add(form.Name);

            foreach (var move in pokemon.Moves)
                detailsMovesNode.Nodes.Add(move.Move.Name);

            foreach (var stat in pokemon.Stats)
                detailsStatsNode.Nodes.Add(stat.Stat.Name);
        }
    }
}