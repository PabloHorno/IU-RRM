using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IU_Windows
{
    public class Usuario
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public Int32 SqlId { get; set; }
        public List<Paciente> GetPacientes()
        {
            return new SQLHelper().GetPacientesFromUser(this);
        }

    }
}
