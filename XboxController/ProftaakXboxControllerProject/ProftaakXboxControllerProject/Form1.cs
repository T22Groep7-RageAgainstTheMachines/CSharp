using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Utilities;
using System.Net.Sockets;
using System.Net;


namespace ProftaakXboxControllerProject
{
    public partial class Form1 : Form
    {
        private Socket sock;
        private IPAddress serverAddr;
        private IPEndPoint endPoint;
        private byte[] send_buffer;
        string lastMessageSent;
        public Form1()
        {
            InitializeComponent();
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            serverAddr = IPAddress.Parse("192.168.1.101");
            endPoint = new IPEndPoint(serverAddr, 2390);
        }
        private void GenerateDataForTransfer()
        {
            /*protocol
             * <kant L><richting F of B><#><value 0-255><%><kant R><richting F of B><#><value 0-255><%>
             * als er een trigger ingedrukt is wordt het <kant L of R + R (Rotate)>
             * als Y knop wordt ingedruk verwisseld de richting <CD> change direction
             * als de B knop wordt ingedruk stopt de RP6 <STOP>
            */
            string dataToSend = "";
            GamePadState controller = GamePad.GetState(PlayerIndex.One);
            int LeftThumbStick = Convert.ToInt32(controller.ThumbSticks.Left.Y * 100);
            int RightThumbStick = Convert.ToInt32(controller.ThumbSticks.Right.X * 100);
            int LeftTrigger = Convert.ToInt32(controller.Triggers.Left * 100);
            int RightTrigger = Convert.ToInt32(controller.Triggers.Right * 100);
            if (LeftThumbStick > 0)
            {
                dataToSend = "MF";
            }
            else if (LeftThumbStick < 0)
            {
                dataToSend = "MB";
            }
            else if (RightThumbStick > 0)
            {
                dataToSend = "MR";
            }
            if (RightTrigger > 50 || LeftTrigger > 50)
            {
                if (RightTrigger > LeftTrigger)
                {
                    dataToSend = "RR";
                }
                else
                {
                    dataToSend = "RL";
                }
            }
            if (controller.Buttons.Y == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                dataToSend = "CD";
            }
            if (controller.Buttons.B == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                dataToSend = "STOP";
            }
            label3.Text = dataToSend;
            if (string.IsNullOrWhiteSpace(dataToSend))
            {
                return;
            }
            if (dataToSend != lastMessageSent)
            {
                send_buffer = Encoding.ASCII.GetBytes(dataToSend);
                sock.SendTo(send_buffer, endPoint);
            }
        }

        private void ResetTextBoxtTexts()
        {
            foreach (TextBox textbox in this.Controls)
            {
                textbox.Text = "";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            GenerateDataForTransfer();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sendMessage("RR");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sendMessage("MF");
        }
        private void sendMessage(string message)
        {
            send_buffer = Encoding.ASCII.GetBytes(message);
            sock.SendTo(send_buffer, endPoint);
        }
    }
}
