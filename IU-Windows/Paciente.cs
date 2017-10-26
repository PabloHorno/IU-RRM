using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IU_Windows
{
    public class Paciente
    {
        public Paciente() { }
        public Paciente(Int32 SqlId) {
            SQLHelper db = new SQLHelper();
            List<Paciente> paciente = db.GetPacientesFromUser(SqlId);
            this.SqlId = paciente[0].SqlId;
            this.Nombre = paciente[0].Nombre;
            this.Apellidos = paciente[0].Apellidos;
        }
        public int SqlId { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
    }
}
