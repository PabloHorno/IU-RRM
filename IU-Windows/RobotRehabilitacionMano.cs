using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace IU_Windows
{
    public class RobotRehabilitacionMano
    {
        public RobotRehabilitacionMano(String portName)
        {
            COM = portName;
        }
        public String COM { get; set; }
        public Int32 baudios { get; } = 9600;
        public SerialPort puertoSerial { get; set; }


    }
}
