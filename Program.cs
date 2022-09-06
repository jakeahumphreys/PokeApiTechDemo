using System;
using System.Windows.Forms;
using PokeApiTool.Data.Validation;

namespace PokeApiTool
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