using System;
using System.Windows.Forms;
using Newtonsoft.Json;
using PokeApiTechDemo.Common;
using PokemonApiClient.Types;

namespace PokeApiTechDemo
{
    public partial class MainForm : Form
    {
        private readonly SearchService _searchService;

        public MainForm()
        {
            InitializeComponent();
            _searchService = new SearchService();
        }
        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            CallSearch(txtSearch.Text);
        }
        
        private void lstHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            CallSearch(lstHistory.SelectedItem.ToString());
        }

        private void CallSearch(string searchText)
        {
            if (!ValidationHelper.IsSearchTextValid(searchText))
            {
                MessageBox.Show("Your search text isn't quite valid. Must be > 3 characters and contain no numbers or special characters.",
                    "Validation Error");
            }
            else
            {
                var searchResult = _searchService.SearchForPokemon(searchText);

                if (searchResult.Pokemon != null)
                {
                    PopulateFormFromResult(searchResult.Pokemon, searchResult.Source);
                }
            }
        }
        
        private void PopulateFormFromResult(Pokemon pokemon, string resultSource)
        {
            lblStatus.Text = "Search complete!";
            lstHistory.Items.Add(pokemon.Name);
            
            txtJson.Text = JsonConvert.SerializeObject(pokemon, Formatting.Indented);

            pbMainImage.ImageLocation = _searchService.SourceImage(pokemon);

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