using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using PokeApiTechDemo.Cache;
using PokeApiTechDemo.Common;
using PokeApiTechDemo.Common.Types;
using PokeApiTechDemo.PokeApi;
using PokeApiTechDemo.PokeApi.Types;
using PokeApiTechDemo.Settings;
using PokeApiTechDemo.Settings.Types;

namespace PokeApiTechDemo
{
    public partial class MainForm : Form
    {
        private readonly PokeApiClient _pokeApiClient;
        private readonly CacheService _cacheService;
        private readonly SettingService _settingService;

        private Dictionary<string, Setting> _settings;

        public MainForm()
        {
            InitializeComponent();
            _pokeApiClient = new PokeApiClient();
            _cacheService = new CacheService(new CacheRepository());
            _settingService = new SettingService(new SettingRepository());
            _settings = _settingService.LoadSettings();
            
            DebugLog("Logging Enabled");
        }

        private void DebugLog(string text)
        {
            if (_settings.TryGetValue("DebugLogging", out var debugLogging));
            
            if(debugLogging != null && _settingService.GetToggleValue(debugLogging))
                Console.WriteLine($"[Debug] {text}");
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
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            
            Pokemon pokemon = null;
            string resultSource;
            
            var potentialCacheEntries = _cacheService.GetCacheEntriesForName(searchText);

            if (potentialCacheEntries.Count > 0)
            {
                var cacheEntry = potentialCacheEntries.First();

                if (cacheEntry.Time.AddMinutes(10) > DateTime.Now)
                {
                    DebugLog("Fetching from cache");
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

            if (pokemon != null)
                PopulateFormFromResult(pokemon, resultSource);
            
            stopwatch.Stop();
            DebugLog($"Fetch took {stopwatch.ElapsedMilliseconds} ms");
        }

        private Pokemon FetchPokemonFromApi(string searchText)
        {
            DebugLog("Fetching from API");
            var result = _pokeApiClient.GetPokemon(searchText);
            if (result.HasError)
            {
                MessageBox.Show(result.Error.Message);
                return null;
            }
            
            var pokemon = result.Pokemon;
            DebugLog("Caching new result");
            _cacheService.CacheResult(pokemon.Name, JsonConvert.SerializeObject(pokemon));

            return pokemon;
        }

        private void PopulateFormFromResult(Pokemon pokemon, string resultSource)
        {
            lblStatus.Text = "Search complete!";
            DebugLog("Populating form from result");
            
            lstHistory.Items.Add(pokemon.Name);
            //Set JSON Tab
            txtJson.Text = JsonConvert.SerializeObject(pokemon, Formatting.Indented);
            
            //Set Image
            pbMainImage.ImageLocation = pokemon.Sprites.FrontDefault;
            
            var randomNumber = Randomiser.GetNumberBetweenOneAndTen();
            DebugLog($"Shiny Random Number: {randomNumber}");
            if (randomNumber == 1)
            {
                DebugLog("Ding, it's a shiny!");
                pbMainImage.ImageLocation = pokemon.Sprites.FrontShiny;
            }
            
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