using System;

namespace IU_Windows
{
    static class Constantes
    {
        static public string Version = "1.0";
        static public string Autor = "Pablo Horno Pérez";
        static public string TituloVentana = $"Robot prototipo de Rehabilitacion de Mano - v{Version} @PHP";
        static public string EnlaceGitHub = "https://github.com/PabloHorno";
        static public Int32 VelocidadComunicacion = 9600;
        static public Int32 TiempoDeBusqueda = 2000; //Milisegundos de busqueda en puerto COM
        static public class Unidades
        {
            static public string Velocidad = "grados/seg";
            static public string Angulo = "grados";
            static public string Tiempo = "seg";
        }
        static public void AcercaDe()
        {
            System.Windows.Forms.MessageBox.Show($"Este es un programa desarollado por\n{Constantes.Autor}", $"Version {Constantes.Version}", 
                System.Windows.Forms.MessageBoxButtons.OK,
                System.Windows.Forms.MessageBoxIcon.Information);
        }

    }
}
