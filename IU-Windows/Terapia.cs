using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IU_Windows
{
    public enum TipoTerapia
    {
        AbrirCerrarDedos,
        AbrirCerrarMano,
        PinzaFina,
        PinzaGruesa
    }
    public class Terapia
    {
        public TipoTerapia tipoTerapia { get; set; }
        public string Nombre
        {
            get {
                return TipoTerapiaToString[tipoTerapia];
            }
        }
        public Dictionary<TipoTerapia, String> TipoTerapiaToString = new Dictionary<TipoTerapia, String> {
            { TipoTerapia.AbrirCerrarDedos, "Abrir y Cerrar Dedos"},
            {TipoTerapia.AbrirCerrarMano, "Abrir y Cerrar Mano" },
            {TipoTerapia.PinzaFina, "Pinza fina" },
            {TipoTerapia.PinzaGruesa, "Pinza gruesa" }
            };

        public int PacienteSqlid { get; set; }
        public int Repeticiones { get; set; }
        public DateTime TiempoApertura { get; set; }
        public DateTime TiempoCierre { get; set; }
        public double VelocidadApertura { get; set; }
        public double VelocidadCierre { get; set; }
        public int LimiteApertura { get; set; }
        public int LimiteCierre { get; set; }
        public string Observacion { get; set; }
    }
}
