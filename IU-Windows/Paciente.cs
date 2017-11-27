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
        public int SqlId { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public DateTime FechaDeNacimiento { get; set; }
        public int Responsable { get; set; }

        private string _Observaciones;

        public string Observaciones
        {
            get { return _Observaciones; }
            set
            {
                _Observaciones = value;
                Dictionary<string, object> param = new Dictionary<string, object> {
                    { "@Observaciones", _Observaciones },
                    { "@Id", this.SqlId }
                };

                new SQLHelper().Query("UPDATE Pacientes SET Observaciones = @Observaciones WHERE Id = @Id", param);
            }
        }

        public List<Terapia> GetTerapias()
        {
            SQLHelper db = new SQLHelper();
            List<Terapia> terapias = db.GetTerapiasFromPaciente(this);
            return terapias;
        }
        public TimeSpan GetHorasTerapias()
        {
            return new SQLHelper().GetHorasTerapiasFromPaciente(this);
        }
    }
}
