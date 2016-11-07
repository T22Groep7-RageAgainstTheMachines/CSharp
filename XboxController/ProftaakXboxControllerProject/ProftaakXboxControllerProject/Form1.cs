using System;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Net.Sockets;
using System.Net;
using BattleBotClient;


namespace ProftaakXboxControllerProject
{
    public partial class Form1 : Form
    {
        private Socket sock;
        private IPAddress serverAddr;
        private IPEndPoint endPoint;
        private byte[] send_buffer;
        string lastMessageSent;
        Client bbc;
        bool gameStarted;
        UdpClient receivingUdpClient;


        public Form1()
        {
            InitializeComponent();
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            serverAddr = IPAddress.Parse("192.168.137.123");
            endPoint = new IPEndPoint(serverAddr, 2390);
            bbc = new Client(7, "192.168.137.1", 5000);
            bbc.GameStarted += Bbc_GameStarted;
            bbc.GamePaused += Bbc_GamePaused;
            bbc.GameStopped += Bbc_GameStopped;
            gameStarted = true;
            lastMessageSent = string.Empty;
            //receivingUdpClient = new UdpClient(11000);
        }

        private void Bbc_GameStopped(object sender, EventArgs e)
        {
            gameStarted = false;
            sendMessage("STOP");
        }

        private void Bbc_GamePaused(object sender, EventArgs e)
        {
            gameStarted = false;
            sendMessage("STOP");
        }

        private void Bbc_GameStarted(object sender, EventArgs e)
        {
            gameStarted = true;
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
            else if (RightThumbStick < 0)
            {
                dataToSend = "ML";
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
            if (controller.Buttons.X == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                dataToSend = "ATTACK";
            }
            //hit target simulator
            if (controller.DPad.Left == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                Console.WriteLine("hit someone");
                bbc.HitSomeone();
            }

            //got hit simulator
            if (controller.DPad.Right == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                Console.WriteLine("got hit");
                bbc.GotHit();
            }
            label3.Text = dataToSend;
            if (string.IsNullOrWhiteSpace(dataToSend))
            {
                return;
            }
            if (dataToSend != lastMessageSent && gameStarted)
            {
                sendMessage("%" + dataToSend + "#");
                System.Console.WriteLine("sending data");
                lastMessageSent = dataToSend;
            }
            else
            {
                System.Console.WriteLine("NOT    sending data");

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
            //receiveMessage();
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
        private void receiveMessage()
        {
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            try
            {
                // Blocks until a message returns on this socket from a remote host.
                Byte[] receiveBytes = receivingUdpClient.Receive(ref RemoteIpEndPoint);
                string receivedData = receiveBytes.ToString();
                MessageBox.Show(receivedData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
