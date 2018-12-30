using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;

namespace ChatClient
{
    class Client
    {
        private string Name { get; }

        private int Port { get; set; }

        private IPAddress IpAddress { get; set; }

        private TcpClient ClientInstance { get; set; }

        public Client(string name)
        {
            // Limit length of name to 32 characters
            if (name.Length > 32) { name = name.Substring(0, 32); }

            // Initialize necessary components for client
            Name = name;
            Port = 8000;
            IpAddress = IPAddress.Parse("192.168.179.47");
        }

        public void Connect()
        {
            try
            {
                // Try connecting to server
                ClientInstance = new TcpClient();
                ClientInstance.Connect(IpAddress, Port);

                // Send name for recognition by server
                Send(Name);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Send(string msg)
        {
            // Avoid messages longer than 512 characters
            if (msg.Length > 512) { msg = msg.Substring(0, 512); }

            try
            {
                // Encode message and write to stream
                Stream stream = ClientInstance.GetStream();
                byte[] bytes = Encoding.UTF8.GetBytes(msg);
                stream.Write(bytes, 0, bytes.Length);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string Receive()
        {
            try
            {
                // Create stream and input buffer
                Stream stream = ClientInstance.GetStream();
                byte[] buffer = new byte[512];

                // Read input
                int n = stream.Read(buffer, 0, buffer.Length);
                return Encoding.UTF8.GetString(buffer, 0, n);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
