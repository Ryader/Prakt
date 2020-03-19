using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Xml.Serialization;
using System.Threading;

namespace Practical1Server
{
    public class UdpFileServer
    {
        private static IPAddress _remoteIpAddress;
        private const int RemotePort = 5002;
        private static readonly UdpClient Sender = new UdpClient();
        private static IPEndPoint _endPoint;

        private static FileStream _fs;

        public static void Main()
        {
            try
            {
                Console.Write("Введите удаленный IP-адрес: ");
                _remoteIpAddress = IPAddress.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
                _endPoint = new IPEndPoint(_remoteIpAddress, RemotePort);

                Console.Write("Введите путь к файлу: ");
                _fs = new FileStream(Console.ReadLine() ?? throw new InvalidOperationException(), FileMode.Open,
                    FileAccess.Read);

                SendFile();

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void SendFile()
        {
            var bytes = new byte[_fs.Length];
            _fs.Read(bytes, 0, bytes.Length);

            Console.WriteLine("Отправка файла размером " + _fs.Length + " байт ...");
            try
            {
                Sender.Send(bytes, bytes.Length, _endPoint);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _fs.Close();
                Sender.Close();
            }

            Console.WriteLine("Файл успешно отправлен.");
            Console.Read();
        }
    }
}
