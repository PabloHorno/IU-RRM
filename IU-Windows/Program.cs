using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IU_Windows
{
    static class Constants
    {
        static public string Version = "1.0";
        static public string Autor = "Pablo Horno Pérez";
        static public string EnlaceGitHub = "https://github.com/PabloHorno";
        static public void AcercaDe()
        {
            MessageBox.Show($"Este es un programa desarollado por\n{Constants.Autor}", $"Version {Constants.Version}", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SeleccionDePaciente(1));
            Application.Run(new SeleccionDePaciente(1));
        }
    }
}
