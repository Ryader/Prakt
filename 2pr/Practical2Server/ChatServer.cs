using System;
using System.Threading;

namespace Practical2Server
{
    public class ChatServer
    {
        private static ServerObject _server;
        private static Thread _listenThread;
        private static void Main()
        {
            try
            {
                _server = new ServerObject();
                _listenThread = new Thread(_server.Listen);
                _listenThread.Start();
            }
            catch (Exception ex)
            {
                _server.Disconnect();
                Console.WriteLine(ex.Message);
            }
        }
    }
}