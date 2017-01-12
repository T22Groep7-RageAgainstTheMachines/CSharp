using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ProftaakXboxControllerProject
{
    public class CommRecUDPFromArduino
    {
        public UdpClient UDPclient { get; private set; }
        public IPAddress ArduinoIPAddress { get; private set; }
        IPEndPoint IPendpoitForArduino { get; set; }
        private int portnumber = 11000;

        public CommRecUDPFromArduino()
        {
            byte[] ArduinoIPAdressBytes = { 192, 168, 1, 252 };
            UDPclient = new UdpClient(portnumber);
            ArduinoIPAddress = new IPAddress(ArduinoIPAdressBytes);
            IPendpoitForArduino = new IPEndPoint(ArduinoIPAddress, portnumber);
        }

        public string ReceiveUDPmessageFromArduino()
        {
            string receivedData = "";
            IPEndPoint endpoint = IPendpoitForArduino;
            try
            {
                if (UDPclient.Available > 0)
                {
                    byte[] receiveBytes = UDPclient.Receive(ref endpoint);
                    string returnData = Encoding.ASCII.GetString(receiveBytes);
                    receivedData = returnData;
                    Console.WriteLine(receivedData);                   
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return receivedData;

        }
    }
}
