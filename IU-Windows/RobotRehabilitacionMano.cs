﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;

namespace IU_Windows
{
    public class RobotRehabilitacionMano
    {
        public RobotRehabilitacionMano(String portName)
        {
            puertoSerial = new SerialPort(portName, Constantes.VelocidadComunicacion);
        }
        public RobotRehabilitacionMano(String portName, Dictionary<string, decimal> parametros)
        {
            this.parametros = parametros;
            NumeroRepeticiones = int.Parse(parametros["repeticiones"].ToString());
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
        Mano mano;

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
                    puertoSerial.Open();
                switch (tipoTerapia)
                {
                    case TipoTerapia.AbrirCerrarMano:

                        break;
                    case TipoTerapia.AbrirCerrarDedos:
                        break;
                    case TipoTerapia.PinzaFina:
                        break;
                    case TipoTerapia.PinzaGruesa:
                        break;
                }
                Thread.Sleep(20);
            }
            if (progreso > NumeroRepeticiones)
                finTerapia = true;
            else
                progreso++;
        }

    }
    public class Mano
    {
        public Dedo Pulgar;
        public Dedo Indice;
        public Dedo Corazon { get; set; }
        public Dedo Anular { get; set; }
        public Dedo Meñique { get; set; }
    }
    public class Dedo
    {
        public float Tiempo { get; set; }
        public float Angulo { get; set; }
        public float Velocidad { get; set; }
    }
}
