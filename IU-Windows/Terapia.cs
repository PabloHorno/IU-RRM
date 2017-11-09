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
        public int PacienteSqlid { get; set; }
        public int Repeticiones { get; set; }
        public DateTime TiempoApertura { get; set; }
        public DateTime TiempoCierre { get; set; }
        public double VelocidadApertura { get; set; }
        public double VelocidadCierre { get; set; }
        public int LimiteApertura { get; set; }
        public int LimiteCierre { get; set; }
    }
}
