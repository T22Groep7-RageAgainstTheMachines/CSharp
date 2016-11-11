using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace PC_to_Arduino_wifi
{
    public class CommunicationReceiveFromArduino
    {
        public UdpClient UDPclient { get; private set; }
        public IPAddress ArduinoIPAddress { get; private set; }
        IPEndPoint IPendpoitForArduino { get; set; }
        private int portnumber = 11000;

        public CommunicationReceiveFromArduino()
        {
            byte[] ArduinoIPAdressBytes = { 192, 168, 1, 110 };
            UDPclient = new UdpClient(portnumber);
            ArduinoIPAddress = new IPAddress(ArduinoIPAdressBytes);
            IPendpoitForArduino = new IPEndPoint(ArduinoIPAddress, portnumber);
        }
    }
}
