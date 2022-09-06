using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;
using PokeApiTool.Common;
using PokemonApiClient.Types;


namespace PokeApiToolWPF
{
    public partial class MainWindow : Window
    {
        private readonly SearchService _searchService;

        public MainWindow()
        {
            InitializeComponent();
            _searchService = new SearchService();
        }

        private void BtnSearch_OnClick(object sender, RoutedEventArgs e)
        {
            CallSearch(txtPokemonName.Text);
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
            
            lblStatus.Content = "Search complete!";
            lstHistory.Items.Add(pokemon.Name);
            
            txtJson.Document.Blocks.Clear();
            txtJson.Document.Blocks.Add(new Paragraph(new Run(JsonConvert.SerializeObject(pokemon, Formatting.Indented))));

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(_searchService.SourceImage(pokemon));
            bitmap.EndInit();
            pbImage.Source = bitmap;
            
            tvDetails.Items.Clear();
            
            var sourceNode = new TreeViewItem();
            sourceNode.Header = "Result Source";
            sourceNode.Items.Add(new TreeViewItem
            {
                Header = resultSource
            });
            tvDetails.Items.Add(sourceNode);

            var detailsNode = new TreeViewItem
            {
                Header = pokemon.Name
            };

            var detailsAbilitiesMetaNode = new TreeViewItem
            {
                Header = "Abilities"
            };

            var detailsFormsNode = new TreeViewItem
            {
                Header = "Forms"
            };

            var detailsMovesNode = new TreeViewItem
            {
                Header = "Moves"
            };

            var detailsStatsNode = new TreeViewItem
            {
                Header = "Stats"
            };

            foreach (var ability in pokemon.Abilities)
            {
                detailsAbilitiesMetaNode.Items.Add(new TreeViewItem
                {
                    Header = ability.Ability.Name
                });
            }
                

            foreach (var form in pokemon.Forms)
            {
                detailsFormsNode.Items.Add(new TreeViewItem
                {
                    Header = form.Name
                });
            }

            foreach (var move in pokemon.Moves)
            {
                detailsMovesNode.Items.Add(new TreeViewItem
                {
                    Header = move.Move.Name
                });
            }

            foreach (var stat in pokemon.Stats)
            {
                detailsStatsNode.Items.Add(new TreeViewItem
                {
                    Header = $"{stat.Stat.Name} : {stat.BaseStat}"
                });
            }
            
            detailsNode.Items.Add(detailsAbilitiesMetaNode);
            detailsNode.Items.Add(detailsFormsNode);
            detailsNode.Items.Add(detailsMovesNode);
            detailsNode.Items.Add(detailsStatsNode);

            tvDetails.Items.Add(detailsNode);
        }

        private void LstHistory_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CallSearch(lstHistory.SelectedItem.ToString());
        }
    }
}