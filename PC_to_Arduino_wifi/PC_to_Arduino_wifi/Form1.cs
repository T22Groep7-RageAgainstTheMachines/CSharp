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
using System.Runtime.InteropServices;
using MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Utilities;

namespace PC_to_Arduino_wifi
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(Keys vKey);
        Communication com;
        int keysdown;

        public Form1()
        {
            InitializeComponent();
            com = new Communication();
            keysdown = 0;
            this.ActiveControl = textBoxTextToSend;
        }

        private void buttonHello_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxTextToSend.Text))
            {
                com.SendData(textBoxTextToSend.Text);
            }

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void textBoxTextToSend_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.W:
                    keysdown++;
                    com.SendData("F");
                    e.Handled = true;
                    break;
                case Keys.A:
                    keysdown++;
                    com.SendData("L");
                    e.Handled = true;
                    break;
                case Keys.S:
                    keysdown++;
                    com.SendData("B");
                    e.Handled = true;
                    break;
                case Keys.D:
                    keysdown++;
                    com.SendData("R");
                    e.Handled = true;
                    break;
            }
        }


        private void textBoxTextToSend_KeyUp(object sender, KeyEventArgs e)
        {
         //   Console.WriteLine(GetAsyncKeyState(Keys.W).ToString());
            switch (e.KeyData)
            {
                case Keys.W:
                    //if (GetAsyncKeyState(Keys.S) != 0)
                    //{
                    //    com.SendData("S");
                    //}
                    keysdown--;
                    e.Handled = true;
                    break;
                case Keys.A:
                    if (GetAsyncKeyState(Keys.W) != 0)
                    {
                        com.SendData("F");
                    }
                    if (GetAsyncKeyState(Keys.S) != 0)
                    {
                        com.SendData("B");
                    }
                    keysdown--;
                    e.Handled = true;
                    break;
                case Keys.S:
                    //if (getasynckeystate(keys.w) != 0)
                    //{
                    //    com.senddata("w");
                    //}
                    keysdown--;
                    e.Handled = true;
                    break;
                case Keys.D:
                    if (GetAsyncKeyState(Keys.W) != 0)
                    {
                        com.SendData("F");
                    }
                    if (GetAsyncKeyState(Keys.S) != 0)
                    {
                        com.SendData("B");
                    }
                    keysdown--;
                    e.Handled = true;
                    break;
            }
            if(keysdown == 0)
            {
                com.SendData("Stop");
            }
        }











        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
        }

        private void backgroundWorker1_DoWork_1(object sender, DoWorkEventArgs e)
        {
         
        }
    }
}
