using System;
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
                    Close();
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
                Close();
            }
        }

        private void ReceiveMessages()
        {
            try
            {
                // Constantly receive messages from client and display in text box
                while (true)
                {
                    AppendTextBox(">> " + client.Receive() + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                // Show message box with error details if connecting fails
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(ex.Message, "Error", buttons);
                Close();
            }
        }

        private void AppendTextBox(string msg)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(AppendTextBox), new object[] { msg });
                return;
            }
            textBox3.Text += msg;
        }
    }
}
