using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;
using Newtonsoft.Json;

namespace IU_Windows
{
    public class RobotRehabilitacionMano
    {
        string messageError = "{\"tipo\":-1}";
        public RobotRehabilitacionMano(String portName, Dictionary<string, decimal> parametros)
        {
            this.parametros = parametros;
            NumeroRepeticiones = int.Parse(parametros["R"].ToString());
            tipoTerapia = (TipoTerapia)int.Parse(parametros["tipo"].ToString());
            puertoSerial = new SerialPort(portName, Constantes.VelocidadComunicacion);
        }
        ~RobotRehabilitacionMano()
        {
            if (puertoSerial != null && puertoSerial.IsOpen)
                puertoSerial.Close();
        }
        public SerialPort puertoSerial { get; set; }
        public Dictionary<string, decimal> parametros { get; set; }
        public bool finTerapia { get; set; } = false;
        public TipoTerapia tipoTerapia { get; set; }
        private bool siguienteMovimiento = true;

        public int NumeroRepeticiones { get; set; }
        public int progreso { get; set; } = 0;
        public int ProgresoRealizado()
        {
            return progreso * 100 / NumeroRepeticiones;
        }
        public void RealizarMovimiento()
        {
            if (finTerapia == false)
            {
                if (!puertoSerial.IsOpen)
                {
                    try
                    {
                        puertoSerial.Open();
                    }
                    catch(Exception e)
                    {
                        System.Windows.Forms.MessageBox.Show(e.Message);
                    }
                }
                if (siguienteMovimiento)
                {
                    siguienteMovimiento = false;
                    puertoSerial.WriteLine(JsonConvert.SerializeObject(parametros));
                    progreso++;
                }
                if (puertoSerial.BytesToRead > 0 && puertoSerial.ReadLine().Contains("NEXT"))
                {
                    siguienteMovimiento = true;
                }
            }
            if (progreso > NumeroRepeticiones)
            {
                finTerapia = true;
                puertoSerial.Close();
            }
        }

        public void Detener()
        {
            if (!puertoSerial.IsOpen)
            {
                try
                {
                    puertoSerial.Open();
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message);
                }
                puertoSerial.WriteLine(messageError);
                puertoSerial.Close();
                finTerapia = true;
            }
        }
    }
}
