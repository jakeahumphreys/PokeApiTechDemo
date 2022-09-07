using System.Windows;
using PokeApiToolWPF.Data.Validation;

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