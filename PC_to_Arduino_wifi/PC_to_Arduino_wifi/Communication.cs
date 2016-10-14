using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;

namespace PC_to_Arduino_wifi
{
    class Communication
    {
        private Socket sock;
        private IPAddress serverAddr;
        private IPEndPoint endPoint;
        public string Message { get; private set; }

        public Communication()
        {

        }

        public bool StartCommunicatie()
        {
            try
            {
                sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                serverAddr = IPAddress.Parse("192.168.0.131");
                endPoint = new IPEndPoint(serverAddr, 2390);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public void SendData(string data)
        {
            if(Message != data)
            {
                //byte[] send_buffer = Encoding.ASCII.GetBytes(data);
                //sock.SendTo(send_buffer, endPoint);
                Console.WriteLine(data);
                Message = data;

            }

        }
    }
}
