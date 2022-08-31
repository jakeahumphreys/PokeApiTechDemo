using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
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
        private readonly SettingService _settingService;

        private Dictionary<string, Setting> _settings;
        private readonly SearchService _searchService;

        public MainForm()
        {
            InitializeComponent();
            _searchService = new SearchService();
            _settingService = new SettingService(new SettingRepository());
            _settings = _settingService.LoadSettings();
        }

        private void DebugLog(string text)
        {
            if (_settings.TryGetValue("DebugLogging", out var debugLogging));
            
            if(debugLogging != null && _settingService.GetToggleValue(debugLogging))
                Console.WriteLine($"[Debug] {text}");
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var searchText = txtSearch.Text;

            if (!ValidationHelper.IsSearchTextValid(searchText))
            {
                MessageBox.Show("Your search text isn't quite valid. Must be > 3 characters and contain no numbers or special characters.",
                    "Validation Error");
            }
            else
            {
               HandleSearch(searchText);
            }
        }

        private void HandleSearch(string searchText)
        {
            var searchResult = _searchService.SearchForPokemon(searchText);

            if (searchResult.Pokemon != null)
            {
                PopulateFormFromResult(searchResult.Pokemon, searchResult.Source);
            }
        }
        
        private void lstHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            var searchText = lstHistory.SelectedItem.ToString();
            txtSearch.Text = searchText;
            lblStatus.Text = "Searching from history...";
            HandleSearch(searchText);
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