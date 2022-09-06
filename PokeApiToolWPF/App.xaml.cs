using System.Windows;
using PokeApiTool.Data.Validation;

namespace PokeApiToolWPF
{
    public partial class App : Application
    {
        public App()
        {
            var dataValidationService = new DataValidationService();
            dataValidationService.PerformDataValidation();
        }
    }
}