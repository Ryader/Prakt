using System;
using System.Net.Sockets;
using System.Text;

namespace Practical2Server
{
    public class ClientObject
    {
        protected internal string Id { get; }
        protected internal NetworkStream Stream { get; private set; }
        private string _userName;
        private readonly TcpClient _client;
        private readonly ServerObject _server;

        public ClientObject(TcpClient tcpClient, ServerObject serverObject)
        {
            Id = Guid.NewGuid().ToString();
            _client = tcpClient;
            _server = serverObject;
            serverObject.AddConnection(this);
        }
        public void Process()
        {
            try
            {
                Stream = _client.GetStream();
                var message = GetMessage();
                _userName = message;

                message = _userName + " вошел в чат";
                _server.BroadcastMessage(message, Id);
                Console.WriteLine(message);
                while (true)
                {
                    try
                    {
                        message = GetMessage();
                        message = $"{_userName}: {message}";
                        Console.WriteLine(message);
                        _server.BroadcastMessage(message, this.Id);
                    }
                    catch
                    {
                        message = $"{_userName}: покинул чат";
                        Console.WriteLine(message);
                        _server.BroadcastMessage(message, this.Id);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                _server.RemoveConnection(Id);
                Close();
            }
        }
        private string GetMessage()
        {
            var data = new byte[64];
            var builder = new StringBuilder();
            do
            {
                var bytes = Stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (Stream.DataAvailable);

            return builder.ToString();
        }
        protected internal void Close()
        {
            Stream?.Close();
            _client?.Close();
        }
    }
}