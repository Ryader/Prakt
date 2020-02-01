using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace _2Tcp
{
    class Server
    {
        public Server(string address, int port)
        {
            Listener = new TcpListener(IPAddress.Parse(address), port);
            Listener.Start();
            Console.WriteLine(Listener.LocalEndpoint);
            while(true)
            {
                TcpClient client = Listener.AcceptTcpClient();
                Thread thread = new Thread(new ParameterizedThreadStart(ClientThread));
                thread.Start(client);
            }
        }

        private void ClientThread(object StateInfo)
        {
            Console.WriteLine((StateInfo as TcpClient).Client.LocalEndPoint);
            new TcpHandler((TcpClient)StateInfo).Process();
           
        }
        
        ~Server()
        {
            if(Listener!=null)
            {
                Listener.Stop();
            }
        }

        TcpListener Listener;
        static void Main(string[] args)
        {
            Server server = new Server("10.0.176.9", 8000);
        }
    }
}
