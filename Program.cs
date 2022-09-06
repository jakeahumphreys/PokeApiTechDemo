using System;
using System.Windows.Forms;
using PokeApiTechDemo.Data.Validation;

namespace PokeApiTechDemo
{
    static class Program
    {
        private static readonly DataValidationService DataValidationService;

        static Program()
        {
            DataValidationService = new DataValidationService();
        }
        [STAThread]
        static void Main()
        {
            DataValidationService.PerformDataValidation();
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}