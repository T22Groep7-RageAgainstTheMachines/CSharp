﻿using System;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Net.Sockets;
using System.Net;
using Client;

namespace ProftaakXboxControllerProject
{
    public partial class Form1 : Form
    {
        //UDP messages from Arduino
        private CommRecUDPFromArduino ArduinoReceiver;
        //
        private Socket sock;
        private IPAddress arduinoAddr;
        private IPEndPoint endPoint;
        private byte[] send_buffer;
        string lastMessageSent;
        bool gameStarted;
        public Form1()
        {
            InitializeComponent();
            ArduinoReceiver = new CommRecUDPFromArduino();
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            arduinoAddr = IPAddress.Parse("192.168.137.123");
            endPoint = new IPEndPoint(arduinoAddr, 2390);
            try
            {
                Client.Client.Connect("PC7", "192.168.137.1", 5000);
                Client.Client.GameStarted += Bbc_GameStarted;
                Client.Client.GamePaused += Bbc_GamePaused;
                Client.Client.GameStopped += Bbc_GameStopped;

            }
            catch (SocketException socketException)
            {
                Console.WriteLine(socketException.Message);
            }
            gameStarted = true;
            lastMessageSent = string.Empty;
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
            string dataToSend = "";
            GamePadState controller = GamePad.GetState(PlayerIndex.One);
            int LeftThumbStickY = Convert.ToInt32(controller.ThumbSticks.Left.Y * 100);
            int LeftThumbStickX = Convert.ToInt32(controller.ThumbSticks.Left.X * 100);
            int RightThumbStick = Convert.ToInt32(controller.ThumbSticks.Right.X * 100);
            int LeftTrigger = Convert.ToInt32(controller.Triggers.Left * 100);
            int RightTrigger = Convert.ToInt32(controller.Triggers.Right * 100);
            if (LeftThumbStickY > 0)
            {
                dataToSend = "MF";
            }
            else if (LeftThumbStickY < 0)
            {
                dataToSend = "MB";
            }
            else if (LeftThumbStickY < 0)
            {
                dataToSend = "ML";
            }
            else if (LeftThumbStickY > 0)
            {
                dataToSend = "MR";
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
                Client.Client.Punt();
            }
            //got hit simulator
            if (controller.DPad.Right == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                Console.WriteLine("got hit");
                Client.Client.Hit();
            }
            label3.Text = dataToSend;
            if (string.IsNullOrWhiteSpace(dataToSend))
            {
                return;
            }
            if (dataToSend != lastMessageSent && gameStarted)
            {
                sendMessage("%" + dataToSend + "#");
                System.Console.WriteLine("sending data" + dataToSend);
                //lastMessageSent = dataToSend;
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
            string receivedData = ArduinoReceiver.ReceiveUDPmessageFromArduino();
            if (!string.IsNullOrWhiteSpace(receivedData))
            {
                Console.WriteLine(receivedData);
            }
            if (receivedData.Contains("GotHit"))
            {
                Client.Client.Hit();
            }
            else if (receivedData.Contains("Hit"))
            {
                Client.Client.Punt();
            }
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
