using System;
using System.Windows.Forms;

namespace PokeApiTechDemo
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Console.WriteLine("App Starting");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}