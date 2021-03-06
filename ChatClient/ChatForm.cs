﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class ChatForm : Form
    {
        Client client;

        public ChatForm()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, MouseEventArgs e)
        {
            if (textBox1.Text != string.Empty)
            {
                // Pass non empty string to client and deactivate components
                client = new Client(textBox1.Text);
                button1.Enabled = false;
                button2.Enabled = true;
                textBox1.Enabled = false;
                textBox2.Enabled = true;

                // Try connecting to the server
                try
                {
                    client.Connect();

                    // Create a new thread to allow asynchronous message reception
                    new Thread(ReceiveMessages).Start();
                }
                catch (Exception ex)
                {
                    // Show message box with error details if connecting fails
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBox.Show(ex.Message, "Error", buttons);
                    CloseForm();
                }
            }
        }

        private void Button2_Click(object sender, MouseEventArgs e)
        {
            try
            {
                // If text box isn't empty send its content to the server
                if (textBox2.Text != string.Empty)
                {
                    client.Send(textBox2.Text);
                    textBox2.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                // Show message box with error details if connecting fails
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(ex.Message, "Error", buttons);
                CloseForm();
            }
        }

        private void ReceiveMessages()
        {
            try
            {
                // Constantly receive messages from client and display in text box
                while (true)
                {
                    // When a non empty message was received display it
                    string msg = client.Receive();
                    if (msg != string.Empty)
                    {
                        AppendTextBox(">> " + msg + Environment.NewLine);
                    }
                }
            }
            catch (Exception ex)
            {
                // Show message box with error details if connecting fails
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(ex.Message, "Error", buttons);
                CloseForm();
            }
        }

        private void AppendTextBox(string msg)
        {
            // Allow access from other threads
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(AppendTextBox), new object[] { msg });
                return;
            }
            textBox3.Text += msg;
        }

        private void CloseForm()
        {
            // Allow other threads to close the form
            if (InvokeRequired)
            {
                BeginInvoke(new Action(CloseForm));
                return;
            }
            Close();
        }

        private void CleanUp(object sender, FormClosingEventArgs e)
        {
            // Properly sever the connection to the server before closing the form
            client.Disconnect();
        }

        private void EnterToSend(object sender, KeyEventArgs e)
        {
            // If enter is pressed send messages
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                Button2_Click(sender, null);
            }
        }
    }
}
