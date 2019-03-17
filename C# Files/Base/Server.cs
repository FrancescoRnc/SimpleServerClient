using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace SimpleServerClient
{
    public class Server
    {
        ITransport transport;

        //EndPoint endPoint;
        //Socket socket;

        string address;
        int port;

        string lastMessage;
        public string LastMessageReceived { get { return lastMessage; } }
        string lastInvertedMessage;
        public string LastInvertedMessage { get { return lastInvertedMessage; } }

        public Server(ITransport transport)
        {          
            this.transport = transport;

        }

        public void Run()
        {
            Console.WriteLine("Server Started");

            while (true)
                Step();
        }

        public virtual void Step()
        {
            EndPoint sender = transport.CreateEndPoint();
            
            byte[] data = transport.Receive(256, ref sender);

            if (data.Length > 0)
            {
                //string message = BitConverter.ToString(data);
                string message = Encoding.UTF8.GetString(data);
                lastMessage = message;
                message = ReverseMessage(message);
                lastInvertedMessage = message;
                Console.WriteLine("Sending message: " + message);

                //Send(message, sender);
                transport.Send(Encoding.UTF8.GetBytes(message.ToCharArray()), sender);
            }

        }

        //public virtual void Receive(byte[] data, EndPoint sender)
        //{
        //   
        //}

        //public virtual void Send(string message, EndPoint sender)
        //{
        //    
        //}

        string ReverseMessage(string message)
        {
            char[] chars = message.ToCharArray();
            Array.Reverse(chars);

            return new string(chars);
        }
    }
}
