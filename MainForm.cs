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
            var result = _pokeApiClient.GetPokemon(txtSearch.Text);

            if (result.HasError)
                MessageBox.Show(result.Error.Message);
            
            _cacheService.CacheResult(result.Pokemon.Name, JsonConvert.SerializeObject(result.Pokemon));
            
            PopulateFormFromResult(result.Pokemon);
        }

        private void PopulateFormFromResult(Pokemon pokemon)
        {
            //Set JSON Tab
            txtJson.Text = JsonConvert.SerializeObject(pokemon, Formatting.Indented);
            
            //Populate Tree View
            tvDetails.Nodes.Clear();
            
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