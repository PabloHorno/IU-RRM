using System;

namespace IU_Windows
{
    static class Constantes
    {
        static public string Version = "1.0";
        static public string Autor = "Pablo Horno Pérez";
        static public string TituloVentana = $"Prototipo de Robot de Rehabilitacion de Mano - v{Version}";
        static public string EnlaceGitHub = "https://github.com/PabloHorno";
        static public Int32 VelocidadComunicacion = 9600;
        static public Int32 TiempoDeBusqueda = 2000; //Milisegundos de busqueda en puerto COM
        static public void AcercaDe()
        {
            System.Windows.Forms.MessageBox.Show($"Este es un programa desarollado por\n{Constantes.Autor}", $"Version {Constantes.Version}", 
                System.Windows.Forms.MessageBoxButtons.OK,
                System.Windows.Forms.MessageBoxIcon.Information);
        }

    }
}
