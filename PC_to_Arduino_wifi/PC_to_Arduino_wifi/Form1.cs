using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;


namespace PC_to_Arduino_wifi
{
    public partial class Form1 : Form
    {
        Socket sock;
        IPAddress serverAddr;
        IPEndPoint endPoint;
        public Form1()
        {
            InitializeComponent();
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            serverAddr = IPAddress.Parse("192.168.0.131");
            endPoint = new IPEndPoint(serverAddr, 2390);


        }

        private void buttonHello_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxTextToSend.Text))
            {
                byte[] send_buffer = Encoding.ASCII.GetBytes(textBoxTextToSend.Text);
                sock.SendTo(send_buffer, endPoint);
            }

        }



    }
}
